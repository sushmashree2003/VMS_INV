using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.IssueManager.TADA;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;
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
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class ISM_ExtensionImpl : Interfaces.ISM_ExtensionInterface
    {

        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public ISM_ExtensionImpl(IssueManagerContext isc, DomainModelMsSqlServerContext db)
        {
            _IssueContext = isc;
            _Context = db;
        }
        public async Task<ISM_ExtensionDTO> getdetails(ISM_ExtensionDTO data)
        {
            try
            {
                //*********************************** All Employee List ********************************// 

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.notifi_chk = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                     if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                   // if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        data.notifi_chk = false;
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_All_EmployeeList";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.get_Emplist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        //*********************************** Extention List ********************************// 
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_DailyReport_ExtentionList";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.get_DailyExtension = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Planner_ExtentionList";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.get_PlannerExtension = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        data.get_month = _IssueContext.IVRM_Month_DMO.Where(m => m.Is_Active == true).Distinct().ToArray();
                    }
                    //IVRMRT_Role


                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_ExtensionDTO get_details(ISM_ExtensionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                data.get_totalotherdays = (from EXT in _IssueContext.ISM_DailyReportGenerationDMO
                                             where (EXT.HRME_Id == data.HRME_Id && EXT.ISMDRPT_ActiveFlg == true && EXT.ISMDRPT_OrdersDateFlg == true && EXT.ISMDRPT_Date.Value.Month == data.IVRM_Month_Id && EXT.ISMDRPT_Date.Value.Year== indianTime0.Year)
                                             select new ISM_ExtensionDTO
                                             {
                                                 CreatedDate = EXT.ISMDRPT_Date
                                             }).Distinct().ToArray();

                //data.get_totalotherdays = _IssueContext.ISM_DailyReportGenerationDMO.Where(d => d.HRME_Id == data.HRME_Id && d.ISMDRPT_ActiveFlg == true && d.ISMDRPT_OrdersDateFlg == true && d.ISMDRPT_Date.Value.Month == data.IVRM_Month_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ISM_ExtensionDTO savedata(ISM_ExtensionDTO data)
        {
            try
            {
                PushNotification pb = new PushNotification(_Context);
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.ExtensionType == "D")
                {
                    //var noofextension = _Context.ISM_DailyReportExtensionDMO.Where(t => t.HRME_Id == data.HRME_Id && t.IVRM_Month_Id==data.IVRM_Month_Id).ToList(); 

                    //if(noofextension.Count>0)
                    //{
                    //    var checktaskcreationresult = _Context.ISM_DailyReportExtensionDMO.Single(a => a.ISMODE_Id == noofextension.FirstOrDefault().ISMODE_Id);
                    //    checktaskcreationresult.ISMODE_Noofdays =(Convert.ToInt64(checktaskcreationresult.ISMODE_Noofdays)+Convert.ToInt64(data.ISMODE_Noofdays)).ToString();
                    //    checktaskcreationresult.UpdatedDate = indianTime0;
                    //    _IssueContext.Update(checktaskcreationresult);                       
                    //}
                    //else
                    //{
                        ISM_DailyReportExtensionDMO dailyExt = new ISM_DailyReportExtensionDMO();
                        dailyExt.HRME_Id = data.HRME_Id;
                        dailyExt.IVRM_Month_Id = data.IVRM_Month_Id;
                        dailyExt.ISMODE_Noofdays = data.ISMODE_Noofdays;
                        dailyExt.ISMODE_Date = indianTime0;
                        dailyExt.ISMODE_Reason = data.ISMODE_Reason;
                        dailyExt.ISMODE_ActiveFlg = true;
                        dailyExt.ISMODE_CreatedBy = data.UserId;
                        dailyExt.ISMODE_UpdatedBy = data.UserId;
                        dailyExt.CreatedDate = indianTime0;
                        dailyExt.UpdatedDate = indianTime0;
                        _IssueContext.Add(dailyExt);
                    //}                  
                }
                else
                {
                    ISM_PlannerExtensionDMO plannerExt = new ISM_PlannerExtensionDMO();
                    plannerExt.HRME_Id = data.HRME_Id;
                    plannerExt.ISMPLE_FromDate = data.ISMPLE_FromDate;
                    plannerExt.ISMPLE_ToDate = data.ISMPLE_ToDate;
                    plannerExt.ISMPLE_Reason = data.ISMPLE_Reason;
                    plannerExt.ISMPLE_ActiveFlg = true;
                    plannerExt.ISMPLE_CreatedBy = data.UserId;
                    plannerExt.ISMPLE_UpdatedBy = data.UserId;
                    plannerExt.CreatedDate = indianTime0;
                    plannerExt.UpdatedDate = indianTime0;
                    _IssueContext.Add(plannerExt);
                }
                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {

                    data.returnval = true;
                    if (data.ExtensionType == "D")
                    {
                        var dailyextidlist = _IssueContext.ISM_DailyReportExtensionDMO.Where(e => e.HRME_Id == data.HRME_Id).OrderByDescending(r => r.ISMODE_Id).Take(1).ToList();
                        if (dailyextidlist.Count>0)
                        {
                            string dd = pb.savewebnotification(data.MI_Id, "DREXTWEB", dailyextidlist.FirstOrDefault().ISMODE_Id, dailyextidlist.FirstOrDefault().ISMODE_Id, data.HRME_Id, data.UserId, "DRExtension");
                        }

                    }
                    else
                    {
                        var dailyextidlist = _IssueContext.ISM_PlannerExtensionDMO.Where(e => e.HRME_Id == data.HRME_Id).OrderByDescending(r => r.ISMPLE_Id).Take(1).ToList();
                        if (dailyextidlist.Count > 0)
                        {
                            string dd = pb.savewebnotification(data.MI_Id, "PLEXTWEB", dailyextidlist.FirstOrDefault().ISMPLE_Id, dailyextidlist.FirstOrDefault().ISMPLE_Id, data.HRME_Id, data.UserId, "PLExtension");
                        }
                    }
                }
                else
                {
                    data.returnval = false;
                }
                if (data.returnval == true)
                {
                    
                    if (data.ExtensionType == "D") {
                        if (data.notifi_chk == true)
                        {

                            var messagestring = "Daily Report Approval STATUS";
                            var text = "Daily Report- Your Request to extend daily report has been appoved";
                            var keylist = _IssueContext.MobileApplAuthenticationDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();
                        //var keylist = _IssueContext.MobileApplAuthenticationDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();
                     
                            var deviceidlist = _Context.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id).ToList();
                            var devicelid = deviceidlist[0].HRME_AppDownloadedDeviceId;
                          // var devicelid= "f63vSfCxKqk:APA91bFR8YXX2apbV-emEfyl74L4uNTWtzND3s_QDs-YqB38SwP00S6n9Ifyl9RwI48J-84LiOu1tcn0bcjVvLemP_I5Q_uSztLue0BkpXAZObMtWrtSl8K6XZEjcvWoOZsqvrWmW8xY";
                            string e = callnotification(devicelid, data.HRME_Id, messagestring, data.MI_Id, keylist[0].MAAN_AuthenticationKey,text,data.UserId);
                        }
                        if (data.email_chk ==true)
                        {
                            var name2 = (from a in _IssueContext.MasterEmployee
                                         where (a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                         select new ISM_ExtensionDTO
                                         {
                                             empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                         }).Distinct().SingleOrDefault();
                            string name = name2.empname.ToString();

                            string mail_id = _IssueContext.Multiple_Email_DMO.Where(t => t.HRME_Id == data.HRME_Id && t.HRMEM_DeFaultFlag == "default").Distinct().SingleOrDefault().HRMEM_EmailId;  
                            string username = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId && t.MI_Id == data.MI_Id).Distinct().SingleOrDefault().IVRMSTAUL_UserName;
                            string Template = "DAILYRPTEXT";
                            sendmail_DAILYRPTEXT(data.MI_Id, name, data.ISMODE_Noofdays, Template, username, mail_id);
                        }
                    }
                    if (data.ExtensionType == "P")
                    {
                        if (data.notifi_chk ==true)
                        {
                            var messagestring = "PLANNER APPROVAL STATUS";
                            var text = "Your Request to extend planner has been appoved";
                            var deviceidlist = _Context.HR_Master_Employee_DMO.Where(t => t.HRME_Id == data.HRME_Id).ToList();
                            var devicelid = deviceidlist[0].HRME_AppDownloadedDeviceId;
                            var keylist = _IssueContext.MobileApplAuthenticationDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();
                           // var devicelid = "f63vSfCxKqk:APA91bFR8YXX2apbV-emEfyl74L4uNTWtzND3s_QDs-YqB38SwP00S6n9Ifyl9RwI48J-84LiOu1tcn0bcjVvLemP_I5Q_uSztLue0BkpXAZObMtWrtSl8K6XZEjcvWoOZsqvrWmW8xY";
                            string e = callnotification(devicelid, data.HRME_Id, messagestring, data.MI_Id,keylist[0].MAAN_AuthenticationKey,text,data.UserId);
                        }
                        if (data.email_chk == true)
                        {

                            var name2 = (from a in _IssueContext.MasterEmployee
                                         where (a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                         select new ISM_ExtensionDTO
                                         {
                                             empname = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                         }).Distinct().SingleOrDefault();
                            string name = name2.empname.ToString();

                            string mail_id = _IssueContext.Multiple_Email_DMO.Where(t => t.HRME_Id == data.HRME_Id && t.HRMEM_DeFaultFlag == "default").Distinct().SingleOrDefault().HRMEM_EmailId;


                            string username = _IssueContext.Staff_User_Login.Where(t => t.Id == data.UserId && t.MI_Id == data.MI_Id).Distinct().SingleOrDefault().IVRMSTAUL_UserName;

                            string Template = "PLANNEREXT";

                            sendmail_PLANNEREXT(data.MI_Id, name, data.ISMPLE_FromDate,data.ISMPLE_ToDate, Template, username, mail_id);
                        }
                    }
                    
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public string callnotification(string devicelid, long HRME_Id,string messagestring, long mi_id,string key,string text,long user_id)
        {
            try
            {
                string sresult;
                string fresult;
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                string url = "";
                string utrrno = "";
                url = "https://fcm.googleapis.com/fcm/send";
                string devicenew1 = '"' + devicelid + '"';

                devicenew1 = "[" + devicenew1 + "]";
                List<string> notificationparams = new List<string>();
                string daata = "";
                long notId = 1;
             
                string sound = "default";
               
                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew1 + "," +
               "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + messagestring + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "body" + '"' + ":" + '"' + text + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());
                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;
                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                    sresult = (string)joresponse1["success"];
                    fresult = (string)joresponse1["failure"];
                }
                string rr = Insertnotification(mi_id, HRME_Id, messagestring, user_id, sresult, fresult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
       
        public string Insertnotification(long MI_Id, long HRME_Id,  string messagestring, long userid, string sr,string fr)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ISM_ExtensionDTO data = new ISM_ExtensionDTO();
                ISM_NotificationsDMO push = new ISM_NotificationsDMO();
                push.MI_Id = MI_Id;
                push.HRME_Id = HRME_Id;
                push.ISMNO_Notification = messagestring;
                push.ISMNO_NoticationDate = indianTime;
                push.ISMNO_NotificationType = "Client";
                if (sr=="1")
                {
                    push.ISMNO_ReadFlg = true;
                }
                else
                {
                    push.ISMNO_ReadFlg = false;

                }
                if (fr == "1")
                {
                    push.ISMNO_MakeUnReadFlg = true;
                }
                else
                {
                    push.ISMNO_MakeUnReadFlg = false;
                }
                push.ISMNO_ActiveFlag = true;
                push.ISMNO_CreatedBy = userid;
                push.ISMNO_UpdatedBy = userid;
                push.CreatedDate = indianTime;
                push.UpdatedDate = indianTime;
                _IssueContext.Add(push);
                var contactExists = _IssueContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.returnvalnot = true;
                }
                else
                {
                    data.returnvalnot = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "successnotif";
        }
        public async void sendmail_DAILYRPTEXT(long MI_Id,string name, string day,string Template,string username,string mail_id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _Context.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                
                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                //DateTime stardate = Convert.ToDateTime(SPCCE_StartDate);
                //string startdate = stardate.ToString("dd'/'mm'/'yyyy");

                val.Add("[NAME]", name);
                val.Add("[DAYS]", day);
                val.Add("[USR]", username);                          

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                            Mailmsg = result_value;
                        }
                    }
                }
                Mailmsg = result_value;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        mailcc = alldetails[0].IVRM_mailcc.ToString();
                    }

                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.
                    try
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                      
                       // mail_id = "shilpakarje@gmail.com"; 
                        message.AddTo(mail_id);

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                       client.SendEmailAsync(message).Wait();

                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Email_Outgoing_For_DAILYRPTEXT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
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
                            Console.WriteLine(ex.Message);
                            // return ex.Message;
                        }

                    }
                }
                //Mails Sending end
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
           // return "success";
        }
        public async void sendmail_PLANNEREXT(long MI_Id, string name, DateTime fromdate,DateTime todate, string Template, string username, string mail_id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _Context.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;

                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                DateTime stardate = Convert.ToDateTime(fromdate);
                string startdate = stardate.ToString("dd'/'mm'/'yyyy");

                DateTime todate1 = Convert.ToDateTime(todate);
                string enddate = todate1.ToString("dd'/'mm'/'yyyy");

                val.Add("[NAME]", name);
                val.Add("[FROMDATE]", startdate);
                val.Add("[TODATE]", enddate);
                val.Add("[USR]", username);

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                            Mailmsg = result_value;
                        }
                    }
                }
                Mailmsg = result_value;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        mailcc = alldetails[0].IVRM_mailcc.ToString();
                    }

                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.
                    try
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                      //  mail_id = "shilpakarje@gmail.com";
                        message.AddTo(mail_id);

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();

                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Email_Outgoing_For_DAILYRPTEXT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mail_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
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
                            Console.WriteLine(ex.Message);
                            // return ex.Message;
                        }

                    }
                }
                //Mails Sending end
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            // return "success";
        }
        public async Task<ISM_ExtensionDTO> getdata(ISM_ExtensionDTO data)
        {
            try
            {
                //*********************************** All Employee List ********************************// 

                var rolet = _IssueContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                var roletype = "";
                data.notifi_chk = true;
                if (rolet.Count > 0)
                {
                    roletype = rolet.FirstOrDefault().IVRMRT_Role;
                    //roletype = "Admin";
                    if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase) || roletype.Equals("HR", StringComparison.OrdinalIgnoreCase) || roletype.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                    // if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        data.notifi_chk = false;
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_EmployeeList_New_TADA";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@Userid",
                     SqlDbType.BigInt)
                            {
                                Value = data.UserId
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.get_Emplist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        //using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        //{
                        //    cmd.CommandText = "ISM_All_EmployeeList";
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    if (cmd.Connection.State != ConnectionState.Open)
                        //        cmd.Connection.Open();
                        //    var retObject = new List<dynamic>();
                        //    try
                        //    {
                        //        using (var dataReader = await cmd.ExecuteReaderAsync())
                        //        {
                        //            while (await dataReader.ReadAsync())
                        //            {
                        //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        //                {
                        //                    dataRow.Add(
                        //                        dataReader.GetName(iFiled),
                        //                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                        //                    );
                        //                }
                        //                retObject.Add((ExpandoObject)dataRow);
                        //            }
                        //        }
                        //        data.get_Emplist = retObject.ToArray();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        Console.WriteLine(ex.Message);
                        //    }
                        //}
                        //*********************************** Extention List ********************************// 
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_DailyReport_ExtentionList";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.get_DailyExtension = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "ISM_Planner_ExtentionList";
                            cmd.CommandType = CommandType.StoredProcedure;
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
                                    {
                                        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                        {
                                            dataRow.Add(
                                                dataReader.GetName(iFiled),
                                               dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                }
                                data.get_PlannerExtension = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        data.get_month = _IssueContext.IVRM_Month_DMO.Where(m => m.Is_Active == true).Distinct().ToArray();
                    }
                    //IVRMRT_Role
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TADA_ExtentionList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.tADA_Extension = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TAADA_ExtentionList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.tAADA_Extension = retObject.ToArray();
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
            return data;
        }
        public ISM_ExtensionDTO savedetails(ISM_ExtensionDTO data)
        {
            try
            {
                PushNotification pb = new PushNotification(_Context);
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.ExtensionType == "TAADA")
                {
                   
                    TADA_PlannerExtensionDMO dailyExt = new TADA_PlannerExtensionDMO();
                    dailyExt.HRME_Id = data.HRME_Id;
                    dailyExt.TADAPLE_FromDate = data.TADAPLE_FromDate;
                    dailyExt.TADAPLE_ToDate = data.TADAPLE_ToDate;
                    dailyExt.TADAPLE_Statusflag = data.ExtensionType;                   
                    dailyExt.TADAPLE_Reason = data.TADAPLE_Reason;
                    dailyExt.TADAPLE_ActiveFlag = true;
                    dailyExt.TADAPLE_CreatedBy = data.UserId;
                    dailyExt.TADAPLE_UpdatedBy = data.UserId;
                    dailyExt.CreatedDate = indianTime0;
                    dailyExt.UpdatedDate = indianTime0;
                    _Context.Add(dailyExt);
                              
                }
                else
                {
                    TADA_PlannerExtensionDMO plannerExt = new TADA_PlannerExtensionDMO();
                    plannerExt.HRME_Id = data.HRME_Id;
                    plannerExt.TADAPLE_FromDate = data.TADAPLE_FromDate;
                    plannerExt.TADAPLE_ToDate = data.TADAPLE_ToDate;
                    plannerExt.TADAPLE_Statusflag = data.ExtensionType;                   
                    plannerExt.TADAPLE_Reason = data.TADAPLE_Reason;
                    plannerExt.TADAPLE_ActiveFlag = true;
                    plannerExt.TADAPLE_CreatedBy = data.UserId;
                    plannerExt.TADAPLE_UpdatedBy = data.UserId;
                    plannerExt.CreatedDate = indianTime0;
                    plannerExt.UpdatedDate = indianTime0;
                    _Context.Add(plannerExt);
                }
                var contactExists = _Context.SaveChanges();
                if (contactExists > 0)
                {

                    data.returnval = true;
                    
                }
                else
                {
                    data.returnval = false;
                }
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
