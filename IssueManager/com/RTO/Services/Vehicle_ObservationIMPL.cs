using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.RTO;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.RTO;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.RTO.Services
{
    public class Vehicle_ObservationIMPL : Interfaces.Vehicle_ObservationInterface
    {
        public RTOContext _RTOContext;
        public DomainModelMsSqlServerContext _db;
        public Vehicle_ObservationIMPL(RTOContext hh, DomainModelMsSqlServerContext db)
        {
            _RTOContext = hh;
            _db = db;
        }
        public VMS_FO_Vehicle_ObservationDTO loaddata(VMS_FO_Vehicle_ObservationDTO data)
        {
            try
            {
                data.vehicalArray = _RTOContext.Master_VehicleDMO.Where(R=>R.TRMV_ActiveFlag==true).Distinct().ToArray();

                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RTO_EMPLOYEE_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                    {
                        Value = "emplist"
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.emplist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                //EMPLIST

                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "RTO_EMPLOYEE_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                    {
                        Value = "FoVlist"
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.ObserVationList = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.CreatedBy = _db.ApplicationUser.Where(R => R.Id == data.UserId).Select(R => R.NormalizedUserName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //RecieptReport
        public VMS_FO_Vehicle_ObservationDTO RecieptReport(VMS_FO_Vehicle_ObservationDTO data)
        {
            try
            {

                data.editlist = _RTOContext.VMS_FO_Vehicle_ObservationDMO.Where(R => R.VMSFOVOB_Id == data.VMSFOVOB_Id).Distinct().ToArray();
                data.filist = _RTOContext.VMS_FO_Vehicle_Observation_FilesDMO.Where(R=>R.VMSFOVOB_Id== data.VMSFOVOB_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VMS_FO_Vehicle_ObservationDTO savedata(VMS_FO_Vehicle_ObservationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.VMSFOVOB_Id > 0)
                {

                    var duplicate = _RTOContext.VMS_FO_Vehicle_ObservationDMO.Where(R => R.HRME_Id == data.HRME_Id && R.TRMV_Id == data.TRMV_Id && R.VMSFOVOB_DateTime == data.VMSFOVOB_DateTime && R.VMSFOVOB_Id != data.VMSFOVOB_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        var obj = _RTOContext.VMS_FO_Vehicle_ObservationDMO.Where(R => R.VMSFOVOB_Id == data.VMSFOVOB_Id).FirstOrDefault();

                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.TRMV_Id = data.TRMV_Id;
                        obj.VMSFOVOB_DateTime = data.VMSFOVOB_DateTime;
                        obj.VMSFOVOB_AnyDamageFlg = data.VMSFOVOB_AnyDamageFlg;
                        obj.VMSFOVOB_DamageDescription = data.VMSFOVOB_DamageDescription;
                        obj.VMSFOVOB_UpdatedDate = DateTime.Now;
                        obj.VMSFOVOB_UpdatedBy = data.UserId;
                        _RTOContext.Update(obj);
                      
                        if (data.Observation_Fileslist != null && data.Observation_Fileslist.Length > 0)
                        {
                            var filelist = _RTOContext.VMS_FO_Vehicle_Observation_FilesDMO.Where(R => R.VMSFOVOB_Id == data.VMSFOVOB_Id).ToList();
                            if (filelist.Count > 0)
                            {
                                foreach (var file in filelist)
                                {
                                    _RTOContext.Remove(file);
                                }
                            }
                            for (int p = 0; p < data.Observation_Fileslist.Length; p++)
                            {
                                VMS_FO_Vehicle_Observation_FilesDMO db = new VMS_FO_Vehicle_Observation_FilesDMO();
                                db.VMSFOVOB_Id = obj.VMSFOVOB_Id;
                                db.VMSFOVOBFL_FileName = data.Observation_Fileslist[p].VMSFOVOBFL_FileName;
                                db.VMSFOVOBFL_FilePath = data.Observation_Fileslist[p].VMSFOVOBFL_FilePath;
                                db.VMSFOVOBFL_Filedesc = data.Observation_Fileslist[p].VMSFOVOBFL_Filedesc;
                                db.VMSFOVOBFL_ActiveFlag = true;
                                db.VMSFOVOBFL_CreatedBy = data.UserId;
                                db.VMSFOVOBFL_UpdatedBy = data.UserId;
                                db.VMSFOVOBFL_CreatedDate = DateTime.Now;
                                db.VMSFOVOBFL_UpdatedDate = DateTime.Now;
                                _RTOContext.Add(db);
                            }


                        }
                        int M = _RTOContext.SaveChanges();
                        if (M > 0)
                        {
                            data.returnval = "update";
                            Dictionary<string, string> val = new Dictionary<string, string>();
                            var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "VEHICLEOBSERVATION" && e.ISES_MailActiveFlag == true).ToList();
                            if (template.Count == 0)
                            {

                                return data;
                            }
                            var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                            string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                            List<Match> variables = new List<Match>();

                            foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                            {
                                variables.Add(match);
                            }
                            Mailmsg = data.Template;
                            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                            alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();

                            string Attechement = "";
                            if (alldetails.Count > 0)
                            {
                                string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                                string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                                string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                                Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                                string Subject = template[0].ISES_MailSubject.ToString();
                                string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                                string mailcc = "";
                                string mailbcc = "";
                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    string[] ccmail = template[0].ISES_MailCCId.Split(',');
                                    mailcc = ccmail[0].ToString();
                                    if (ccmail.Length > 1)
                                    {
                                        if (ccmail[1] != null || ccmail[1] != "")
                                        {
                                            mailbcc = ccmail[1].ToString();
                                        }
                                    }
                                }
                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    //Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                                }

                                var message = new SendGridMessage();
                                message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                                message.Subject = Subject;                            
                                string emailid = _db.Multiple_Email_DMO.Where(L => L.HRME_Id == data.HRME_Id && L.HRMEM_DeFaultFlag == "default").Select(L => L.HRMEM_EmailId).FirstOrDefault();
                                long Id = _db.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                                message.AddTo(emailid);

                                if (data.Observation_Fileslist.Length > 0)
                                {
                                    foreach (var item in data.Observation_Fileslist)
                                    {
                                        if (item.VMSFOVOBFL_FilePath != null && item.VMSFOVOBFL_FilePath != "")
                                        {
                                            var webClient = new WebClient();
                                            byte[] imageBytes = webClient.DownloadData(item.VMSFOVOBFL_FilePath);                                            
                                            string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);                                         
                                            message.AddAttachment(item.VMSFOVOBFL_FilePath, fileContentsAsBase64, null, null, null);
                                        }
                                    }
                                }
                                if (mailcc != null && mailcc != "")
                                {
                                    message.AddCc(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    message.AddBcc(mailbcc);
                                }
                                message.HtmlContent = Mailmsg;
                                var client = new SendGridClient(sengridkey);
                                client.SendEmailAsync(message).Wait();
                                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                                {
                                    var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "VEHICLEOBSERVATION" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                                    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                                    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                    cmd.CommandText = "IVRM_Email_Outgoing";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                                    {
                                        Value = emailid
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                                    {
                                        Value = Mailmsg
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                                    {
                                        Value = modulename[0]
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = data.MI_Id
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

                                    }
                                }
                            }
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    var duplicate = _RTOContext.VMS_FO_Vehicle_ObservationDMO.Where(R => R.HRME_Id == data.HRME_Id && R.TRMV_Id == data.TRMV_Id && R.VMSFOVOB_DateTime == data.VMSFOVOB_DateTime && R.VMSFOVOB_AnyDamageFlg == data.VMSFOVOB_AnyDamageFlg && R.VMSFOVOB_DamageDescription == data.VMSFOVOB_DamageDescription).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "duplicate";
                    }
                    else
                    {
                        VMS_FO_Vehicle_ObservationDMO obj = new VMS_FO_Vehicle_ObservationDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.HRME_Id = data.HRME_Id;
                        obj.TRMV_Id = data.TRMV_Id;
                        obj.VMSFOVOB_DateTime = data.VMSFOVOB_DateTime;
                        obj.VMSFOVOB_AnyDamageFlg = data.VMSFOVOB_AnyDamageFlg;
                        obj.VMSFOVOB_DamageDescription = data.VMSFOVOB_DamageDescription;
                        obj.VMSFOVOB_ActiveFlag = true;
                        obj.VMSFOVOB_CreatedDate = DateTime.Now;
                        obj.VMSFOVOB_UpdatedDate = DateTime.Now;
                        obj.VMSFOVOB_CreatedBy = data.UserId;
                        obj.VMSFOVOB_UpdatedBy = data.UserId;
                        _RTOContext.Add(obj);
                        if (data.Observation_Fileslist != null && data.Observation_Fileslist.Length > 0)
                        {
                            for (int p = 0; p < data.Observation_Fileslist.Length; p++)
                            {
                                VMS_FO_Vehicle_Observation_FilesDMO db = new VMS_FO_Vehicle_Observation_FilesDMO();
                                db.VMSFOVOB_Id = obj.VMSFOVOB_Id;
                                db.VMSFOVOBFL_FileName = data.Observation_Fileslist[p].VMSFOVOBFL_FileName;
                                db.VMSFOVOBFL_FilePath = data.Observation_Fileslist[p].VMSFOVOBFL_FilePath;
                                db.VMSFOVOBFL_Filedesc = data.Observation_Fileslist[p].VMSFOVOBFL_Filedesc;
                                db.VMSFOVOBFL_ActiveFlag = true;
                                db.VMSFOVOBFL_CreatedBy = data.UserId;
                                db.VMSFOVOBFL_UpdatedBy = data.UserId;
                                db.VMSFOVOBFL_CreatedDate = DateTime.Now;
                                db.VMSFOVOBFL_UpdatedDate = DateTime.Now;
                                _RTOContext.Add(db);

                            }
                          

                        }
                        int T = _RTOContext.SaveChanges();
                        if (T > 0)
                        {

                            data.returnval = "save";
                            Dictionary<string, string> val = new Dictionary<string, string>();
                            var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "VEHICLEOBSERVATION" && e.ISES_MailActiveFlag == true).ToList();
                            if (template.Count == 0)
                            {

                                return data;
                            }
                            var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                            string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                            List<Match> variables = new List<Match>();

                            foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                            {
                                variables.Add(match);
                            }
                            Mailmsg = data.Template;
                            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                            alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();

                            string Attechement = "";
                            if (alldetails.Count > 0)
                            {
                                string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                                string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                                string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                                Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                                string Subject = template[0].ISES_MailSubject.ToString();
                                string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                                string mailcc = "";
                                string mailbcc = "";
                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    string[] ccmail = template[0].ISES_MailCCId.Split(',');
                                    mailcc = ccmail[0].ToString();
                                    if (ccmail.Length > 1)
                                    {
                                        if (ccmail[1] != null || ccmail[1] != "")
                                        {
                                            mailbcc = ccmail[1].ToString();
                                        }
                                    }
                                }
                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                  
                                }

                                var message = new SendGridMessage();
                                message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                                message.Subject = Subject;                               
                                string emailid = _db.Multiple_Email_DMO.Where(L => L.HRME_Id == data.HRME_Id && L.HRMEM_DeFaultFlag == "default").Select(L => L.HRMEM_EmailId).FirstOrDefault();
                                long Id = _db.Staff_User_Login.Where(t => t.Id == data.UserId).Select(t => t.Emp_Code).FirstOrDefault();
                                message.AddTo(emailid);

                                if (data.Observation_Fileslist.Length > 0)
                                {
                                    foreach (var item in data.Observation_Fileslist)
                                    {
                                        if (item.VMSFOVOBFL_FilePath != null && item.VMSFOVOBFL_FilePath != "")
                                        {
                                            var webClient = new WebClient();
                                            byte[] imageBytes = webClient.DownloadData(item.VMSFOVOBFL_FilePath);
                                            // byte[] bytes = File.ReadAllBytes(a);
                                            string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                            //message.AddAttachment(item.cfilepath, "", null, null, null);
                                            message.AddAttachment(item.VMSFOVOBFL_FilePath, fileContentsAsBase64, null, null, null);
                                        }
                                    }
                                }
                                if (mailcc != null && mailcc != "")
                                {
                                    message.AddCc(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    message.AddBcc(mailbcc);
                                }
                                message.HtmlContent = Mailmsg;
                                var client = new SendGridClient(sengridkey);
                                client.SendEmailAsync(message).Wait();
                                using (var cmd = _RTOContext.Database.GetDbConnection().CreateCommand())
                                {
                                    var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "VEHICLEOBSERVATION" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                                    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                                    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                                    cmd.CommandText = "IVRM_Email_Outgoing";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                                    {
                                        Value = emailid
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                                    {
                                        Value = Mailmsg
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                                    {
                                        Value = modulename[0]
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = data.MI_Id
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

                                    }
                                }
                            }
                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }

                }
                //VMS_FO_Vehicle_ObservationDMO


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //onchange
        public VMS_FO_Vehicle_ObservationDTO onchange(VMS_FO_Vehicle_ObservationDTO data)
        {
            try
            {
                if (data.VMSFOVOB_Id > 0)
                {
                    var obj = _RTOContext.VMS_FO_Vehicle_ObservationDMO.Where(R => R.VMSFOVOB_Id == data.VMSFOVOB_Id).FirstOrDefault();
                    if (obj.VMSFOVOB_ActiveFlag == true)
                    {
                        obj.VMSFOVOB_ActiveFlag = false;
                    }
                    else
                    {
                        obj.VMSFOVOB_ActiveFlag = true;
                    }

                    obj.VMSFOVOB_UpdatedDate = DateTime.Now;
                    obj.VMSFOVOB_UpdatedBy = data.UserId;
                    _RTOContext.Update(obj);
                    int i = _RTOContext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = "active";
                    }
                    else
                    {
                        data.returnval = "notactive";
                    }
                }
                else
                {
                    data.returnval = "admin";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public VMS_FO_Vehicle_ObservationDTO savedataSource(VMS_FO_Vehicle_ObservationDTO data)        {            try            {



            }            catch (Exception e)            {                Console.WriteLine(e.Message);                data.returnval = "admin";            }            return data;        }

        public VMS_FO_Vehicle_ObservationDTO clientDecative(VMS_FO_Vehicle_ObservationDTO data)        {            try            {

            }            catch (Exception f)            {                Console.WriteLine(f.Message);                data.returnval = "admin";            }            return data;        }
    }
}
