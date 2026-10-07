using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
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
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class VM_WebJob_SchedulerImpl : Interfaces.VM_WebJob_SchedulerInterface
    {
        public VisitorsManagementContext _visctxt;
        public DomainModelMsSqlServerContext _db;

        public VM_WebJob_SchedulerImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext dbctxt)
        {
            _visctxt = context;
            _db = dbctxt;
        }
        public VM_webJob_SchedulerDTO vmwebjobscheduler(VM_webJob_SchedulerDTO data)
        {
            try
            {
                var getconfiguration = _visctxt.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.IVRMGC_VMReminderFlag == data.typeflag
                && a.IVRMGC_VMReminderSchedule == data.type).ToList();

                if (getconfiguration.Count > 0 && getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag != null
                    && getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag != "")
                {
                    string SchType = getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag;
                    string Diff = getconfiguration.FirstOrDefault().IVRMGC_VMReminderSchedule;

                    List<VM_webJob_SchedulerDTO> result1 = new List<VM_webJob_SchedulerDTO>();

                    using (var cmd = _visctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VM_Appointment_ScheduledEmpsList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SchType", SqlDbType.VarChar)
                        {
                            Value = SchType
                        });
                        cmd.Parameters.Add(new SqlParameter("@Diff", SqlDbType.VarChar)
                        {
                            Value = Diff
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
                                    result1.Add(new VM_webJob_SchedulerDTO
                                    {
                                        visitorid = Convert.ToInt64(dataReader["AMST_Id"]),
                                        visitorname = (dataReader["VMAP_VisitorName"]).ToString(),
                                        mobileno = Convert.ToInt64(dataReader["VMAP_VisitorContactNo"]),
                                        emailid = dataReader["VMAP_VisitorEmailid"].ToString(),
                                        datetime = (dataReader["VMAP_MeetingDateTime"]).ToString()
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    // Visitor Details
                    for (int i = 0; i < result1.Count; i++)
                    {
                        long visitorid = result1[i].visitorid;
                        string visitorname = result1[i].visitorname;
                        long visitormobileno = result1[i].mobileno;
                        string visitoremailid = result1[i].emailid;
                        string visitordatetime = result1[i].datetime;

                        try
                        {
                            var x = sendSms(data.MI_Id, visitormobileno, "VISITOR_APPOITMENT_SCHEDULER", visitorid, visitordatetime, visitorname);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        try
                        {
                            string s = SendEmail(data.MI_Id, visitoremailid, "VISITOR_APPOITMENT_SCHEDULER", visitorid, visitordatetime, visitorname);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    data.result = "No Scheduler";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VM_webJob_SchedulerDTO vmhourwebjobscheduler(VM_webJob_SchedulerDTO data)
        {
            try
            {
                var getconfiguration = _visctxt.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.IVRMGC_VMReminderFlag == "Hour"
                && a.IVRMGC_VMReminderSchedule == data.type).ToList();

                if (getconfiguration.Count > 0 && getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag != null
                    && getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag != "")
                {
                    string SchType = getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag;
                    string Diff = getconfiguration.FirstOrDefault().IVRMGC_VMReminderSchedule;

                    List<VM_webJob_SchedulerDTO> result1 = new List<VM_webJob_SchedulerDTO>();

                    using (var cmd = _visctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VM_Appointment_ScheduledEmpsList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SchType", SqlDbType.VarChar)
                        {
                            Value = SchType
                        });
                        cmd.Parameters.Add(new SqlParameter("@Diff", SqlDbType.VarChar)
                        {
                            Value = Diff
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
                                    result1.Add(new VM_webJob_SchedulerDTO
                                    {
                                        visitorid = Convert.ToInt64(dataReader["AMST_Id"]),
                                        visitorname = (dataReader["VMAP_VisitorName"]).ToString(),
                                        mobileno = Convert.ToInt64(dataReader["VMAP_VisitorContactNo"]),
                                        emailid = dataReader["VMAP_VisitorEmailid"].ToString(),
                                        datetime = (dataReader["VMAP_MeetingDateTime"]).ToString()
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    // Visitor Details
                    for (int i = 0; i < result1.Count; i++)
                    {
                        long visitorid = result1[i].visitorid;
                        string visitorname = result1[i].visitorname;
                        long visitormobileno = result1[i].mobileno;
                        string visitoremailid = result1[i].emailid;
                        string visitordatetime = result1[i].datetime;

                        try
                        {
                            var x = sendSms(data.MI_Id, visitormobileno, "VISITOR_APPOITMENT_SCHEDULER", visitorid, visitordatetime, visitorname);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        try
                        {
                            string s = SendEmail(data.MI_Id, visitoremailid, "VISITOR_APPOITMENT_SCHEDULER", visitorid, visitordatetime, visitorname);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    data.result = "No Scheduler";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public VM_webJob_SchedulerDTO vmminwebjobscheduler(VM_webJob_SchedulerDTO data)
        {
            try
            {
                var getconfiguration = _visctxt.GeneralConfigDMO.Where(a => a.MI_Id == data.MI_Id && a.IVRMGC_VMReminderFlag == "Mins"
                && a.IVRMGC_VMReminderSchedule == data.type).ToList();

                if (getconfiguration.Count > 0 && getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag != null
                    && getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag != "")
                {
                    string SchType = getconfiguration.FirstOrDefault().IVRMGC_VMReminderFlag;
                    string Diff = getconfiguration.FirstOrDefault().IVRMGC_VMReminderSchedule;

                    List<VM_webJob_SchedulerDTO> result1 = new List<VM_webJob_SchedulerDTO>();

                    using (var cmd = _visctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VM_Appointment_ScheduledEmpsList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SchType", SqlDbType.VarChar)
                        {
                            Value = SchType
                        });
                        cmd.Parameters.Add(new SqlParameter("@Diff", SqlDbType.VarChar)
                        {
                            Value = Diff
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
                                    result1.Add(new VM_webJob_SchedulerDTO
                                    {
                                        visitorid = Convert.ToInt64(dataReader["AMST_Id"]),
                                        visitorname = (dataReader["VMAP_VisitorName"]).ToString(),
                                        mobileno = Convert.ToInt64(dataReader["VMAP_VisitorContactNo"]),
                                        emailid = dataReader["VMAP_VisitorEmailid"].ToString(),
                                        datetime = (dataReader["VMAP_MeetingDateTime"]).ToString()
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    // Visitor Details
                    for (int i = 0; i < result1.Count; i++)
                    {
                        long visitorid = result1[i].visitorid;
                        string visitorname = result1[i].visitorname;
                        long visitormobileno = result1[i].mobileno;
                        string visitoremailid = result1[i].emailid;
                        string visitordatetime = result1[i].datetime;

                        try
                        {
                            var x = sendSms(data.MI_Id, visitormobileno, "VISITOR_APPOITMENT_SCHEDULER", visitorid, visitordatetime, visitorname);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        try
                        {
                            string s = SendEmail(data.MI_Id, visitoremailid, "VISITOR_APPOITMENT_SCHEDULER", visitorid, visitordatetime, visitorname);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    data.result = "No Scheduler";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID, string date, string name)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    result = result.Replace("[NAME]", name);
                    result = result.Replace("[DATE]", date);
                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id", SqlDbType.VarChar)
                        {
                            Value = messageid
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
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string SendEmail(long MI_Id, string mail_id, string Template, long UserID, string date, string name)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    Mailmsg = Mailmsg.Replace("[NAME]", name);
                    Mailmsg = Mailmsg.Replace("[DATE]", date);
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
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
                        message.AddTo(mail_id);

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
                    }
                    else
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

                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {
                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {
                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }
                                        }
                                    }
                                }

                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Value = MI_Id
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
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}
