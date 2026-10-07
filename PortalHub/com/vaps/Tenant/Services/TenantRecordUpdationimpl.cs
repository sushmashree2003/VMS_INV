using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Tenants;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Tenant.Services
{
    public class TenantRecordUpdationimpl:Interface.TenantRecordUpdationInterface
    {

        private static ConcurrentDictionary<string, TenantDashboardDTO> _login =
           new ConcurrentDictionary<string, TenantDashboardDTO>();
        private PortalContext _portalDashboardContext;
        private readonly DomainModelMsSqlServerContext _db;

        public TenantRecordUpdationimpl(PortalContext portalDashboardContext , DomainModelMsSqlServerContext db)
        {
            _portalDashboardContext = portalDashboardContext;
            _db = db;
        }
        public TenantDashboardDTO ViewTenantProfile(TenantDashboardDTO data)
        {
            try
            {

                    data.tenants_details=_portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.User_Id).ToArray();
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //anvesha

        public TenantDashboardDTO Editrenewal(TenantDashboardDTO data)
        {
            // data.tenants_details = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_UserId == data.TENANTAGR_UserId).ToArray();
            var updates = _portalDashboardContext.TenantAgreement.Where(t => t.TENANTAGR_Id == data.TENANTAGR_Id).FirstOrDefault();

            updates.TENANTAGR_TenantName = data.TENANTAGR_TenantName;
            updates.TENANTAGR_Address = data.TENANTAGR_Address;
            updates.TENANTAGR_NoOfOccupants = data.TENANTAGR_NoOfOccupants;
            updates.TENANTAGR_ContactNo = data.TENANTAGR_ContactNo;
            updates.TENANTAGR_EmailId = data.TENANTAGR_EmailId;
            _portalDashboardContext.Update(updates);
            var i= _portalDashboardContext.SaveChanges();

            var building = _portalDashboardContext.HR_Master_Building.Where(t => t.HRMB_Id == updates.HRMB_Id).FirstOrDefault();
            ////building.HRMB_BuildingName = data.HRMB_BuildingName;
            var floor = _portalDashboardContext.HR_Master_Floor.Where(t => t.HRMB_Id == building.HRMB_Id).FirstOrDefault();
           // floor.HRMF_FloorName = data.HRMF_FloorName;
            var room = _portalDashboardContext.HR_Master_Room.Where(t => t.HRMR_Id == updates.HRMR_Id && t.HRMF_Id == floor.HRMF_Id).FirstOrDefault();
           // room.HRMR_RoomName = data.HRMR_RoomName;
            

            if (i > 0)
            {
                data.returnval = "true";
                data.message = "Add";
              
                sendmailToManagement("TENANTRECORDUPDATE", data.TENANTAGR_TenantName,data.TENANTAGR_Address, data.TENANTAGR_NoOfOccupants,data.TENANTAGR_EmailId, data.TENANTAGR_ContactNo, data.MI_Id, data.TENANTAGR_Id,building.HRMB_BuildingName,floor.HRMF_FloorName,room.HRMR_RoomName);

            }
            else
            {
                data.returnval = "false";
                data.message = "Error";
              
            }
            return data;
        }


        public string sendmailToManagement(string Template, string Username, string address, long NoOfOccupants, string email, string contactnumber, long mi_id, long TENANTAGR_Id, string buildingname, string floorname,string roomname)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == mi_id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = Mailmsg.Replace("[NAME]", Username);
                Mailmsg = Mailmsg.Replace("[ADDRESS]", address);
                Mailmsg = Mailmsg.Replace("[EMAIL]", email);
                Mailmsg = Mailmsg.Replace("[CONTACT]", contactnumber);
                Mailmsg = Mailmsg.Replace("[NOOFOCC]", (NoOfOccupants).ToString());
                Mailmsg = Mailmsg.Replace("[BUILDING]", buildingname);
                Mailmsg = Mailmsg.Replace("[FLOOR]", floorname);
                Mailmsg = Mailmsg.Replace("[ROOM]", roomname);
                Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(mi_id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(mi_id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)
    ;


                        if (Attechement.Equals("1"))
                        {
                            var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }




                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(email)
    ;
                        //****************** EMAIL CC DETAILS ***************//

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //****************** EMAIL BCC DETAILS ***************//

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            string[] ccmaildetails = template[0].ISES_MailBCCId.Split(',');

                            foreach (var d in ccmaildetails)
                            {
                                message.AddCc(d);
                            }
                        }

                        message.HtmlContent = Mailmsg;
                        var client = new SendGridClient(sengridkey);
                        try
                        {
                            client.SendEmailAsync(message).Wait();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }




                    }


                    using (var cmd = _portalDashboardContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = (mi_id.ToString())
                     
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "success";


        }


    }

}
