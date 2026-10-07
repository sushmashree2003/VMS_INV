using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;

using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IssueManager.com.vaps.Services
{
    public class VMS_SendNoticeImpl : Interface.VMS_SendNoticeInterface
    {

        public DomainModelMsSqlServerContext _db;

        public IssueManagerContext _context;

        public VMS_SendNoticeImpl(IssueManagerContext _cont, DomainModelMsSqlServerContext db)
        {
            _context = _cont;
            _db = db;
        }
        public VMS_SendNoticeDTO Getdetails(VMS_SendNoticeDTO data)
        {
            try
            {

                data.employeelist = (from a in _context.MasterEmployee
                                     where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new VMS_SendNoticeDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         MI_Id = a.MI_Id,
                                         HRME_EmployeeFirstName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? "" : a.HRME_EmployeeLastName),
                                         HRME_EmailId = a.HRME_EmailId

                                     }).ToArray();

                _context.MasterEmployee.Where(a => a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false).ToArray();

                data.vmsnoticelist = (from a in _context.VMS_SendNoticeDMO
                                      from b in _context.MasterEmployee
                                      where (a.MI_Id == b.MI_Id && a.HRME_Id == b.HRME_Id && a.VNB_ActiveFlag == true && b.HRME_ActiveFlag == true)
                                      select new VMS_SendNoticeDTO
                                      {
                                          HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? "" : b.HRME_EmployeeLastName),
                                          VNB_ID = a.VNB_ID,
                                          VNB_Title = a.VNB_Title,
                                          VNB_Description = a.VNB_Description,
                                          VNB_CreatedDate = a.VNB_CreatedDate,
                                          VNB_FileName = a.VNB_FileName,
                                          VNB_FilePath = a.VNB_FilePath,
                                          VNB_ActiveFlag = a.VNB_ActiveFlag
                                      }
                                      ).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_SendNoticeDTO gettenant(VMS_SendNoticeDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_SendNoticeDTO savedetail(VMS_SendNoticeDTO data)
        {
            try
            {

                if (data.VNB_ID > 0)
                {

                    var resultobj = _context.VMS_SendNoticeDMO.Single(t => t.VNB_ID.Equals(data.VNB_ID) && t.MI_Id.Equals(data.MI_Id));

                    resultobj.VNB_Title = data.VNB_Title;
                    resultobj.VNB_Description = data.VNB_Description;
                    resultobj.VNB_Type = data.VNB_Type;
                    resultobj.VNB_ActiveFlag = true;
                    resultobj.VNB_FileName = data.VNB_FileName;
                    resultobj.VNB_FilePath = data.VNB_FilePath;
                    resultobj.VNB_UpdatedDate = DateTime.Now;
                    resultobj.VNB_UpdatedBy = data.UserId;

                    _context.Update(resultobj);

                    var contactExists = _context.SaveChanges();
                    if (contactExists > 0)
                    {

                        data.message = "Update";
                    }
                    else
                    {

                        data.message = "error";
                    }

                }

                else
                {
                    for (int i = 0; i < data.studentarray.Length; i++)
                    {
                        var checkduplicate = _context.VMS_SendNoticeDMO.Where(t => t.HRME_Id == data.studentarray[i].HRME_Id && t.VNB_Title == data.VNB_Title).ToList();
                        if (checkduplicate.Count == 0)
                        {

                            VMS_SendNoticeDMO resultobj = new VMS_SendNoticeDMO();
                            resultobj.VNB_Title = data.VNB_Title;
                            resultobj.VNB_Description = data.VNB_Description;
                            resultobj.MI_Id = data.studentarray[i].MI_Id;
                            resultobj.HRME_Id = data.studentarray[i].HRME_Id;
                            resultobj.VNB_Type = data.VNB_Type;
                            resultobj.VNB_ActiveFlag = true;
                            resultobj.VNB_FileName = data.FilePath_Array[0].VNB_FileName;
                            resultobj.VNB_FilePath = data.FilePath_Array[0].VNB_FilePath;
                            resultobj.VNB_CreatedDate = data.VNB_CreatedDate;
                            resultobj.VNB_UpdatedDate = DateTime.Now;
                            resultobj.VNB_CreatedBy = data.UserId;
                            resultobj.VNB_UpdatedBy = data.UserId;

                            _context.Add(resultobj);

                            var contactExists = _context.SaveChanges();
                            if (contactExists > 0)
                            {
                                data.message = "Add";
                                sendmailToManagement("VMSSENDNOTICE", data.studentarray[i].HRME_EmailId, data.studentarray[i].HRME_EmployeeFirstName, data.VNB_Title, data.VNB_Description, data.FilePath_Array[0].VNB_FilePath, data.studentarray[i].MI_Id);
                            }
                            else
                            {
                                data.message = "error";
                            }
                        }
                    }


                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public string sendmailToManagement(string Template, string mail_id, string EmployeeName, string NoticeTitle, string NoticeDescription, string files, long mi_id)
        {
            try
            {
                DateTime now = DateTime.Now;
                string strDate = now.ToString("dd-MM-yyyy");

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == mi_id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                Mailmsg = Mailmsg.Replace("[EMPNAME]", EmployeeName);
                Mailmsg = Mailmsg.Replace("[TITLE]", NoticeTitle);
                Mailmsg = Mailmsg.Replace("[DESCRIPTION]", NoticeDescription);
                Mailmsg = Mailmsg.Replace("[DATE1]", strDate);
                Mailcontent = Template;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(mi_id)).ToList();

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
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(mi_id)).ToList();


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
                    message.AddTo(mail_id);
                    //  message.AddTo("kavitajnandoor21@gmail.com");

                    if (files != null && files != "")
                    {
                        var webClient = new WebClient();
                        byte[] imageBytes = webClient.DownloadData(files);

                        string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);

                        message.AddAttachment(files, fileContentsAsBase64, null, null, null);
                    }

                    if (Attechement.Equals("1"))
                    {
                        var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                    //******************* EMAIL CC DETAILS ****************//

                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }

                    //******************** Attachment ******************* //





                    //******************* EMAIL BCC DETAILS ****************//




                    //if (mailcc != null && mailcc != "")
                    //{
                    //    message.AddCc(mailcc);
                    //}
                    //if (mailbcc != null && mailbcc != "")
                    //{
                    //    message.AddBcc(mailbcc);
                    //}

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



                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.smsEmailSetting.Where(e => e.MI_Id == mi_id && e.ISES_Template_Name == "VMSSENDNOTICE" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
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
                            Value = mi_id
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




        public VMS_SendNoticeDTO editdetails(VMS_SendNoticeDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_SendNoticeDTO deactivate(VMS_SendNoticeDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public VMS_SendNoticeDTO viewData(VMS_SendNoticeDTO dto)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }


    }
}
