using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
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
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Services
{
    public class EmailDeviationImpl : Interfaces.EmailDeviationInterface
    {
        public IssueManagerContext _IssueContext;
        public DomainModelMsSqlServerContext _Context;
        public EmailDeviationImpl(IssueManagerContext para, DomainModelMsSqlServerContext para2)
        {
            _IssueContext = para;
            _Context = para2;
        }

        public EmailDeviationDTO sendeach(EmailDeviationDTO data)
        {
            List<EmailDeviationDTO> emplist = new List<EmailDeviationDTO>();
            try
            {
                DateTime datetimennow = DateTime.Now;
                datetimennow = Convert.ToDateTime(datetimennow.Date.ToString("yyyy-MM-dd"));
                long lyear_id = 0;
                var yearid = _IssueContext.HR_Master_LeaveYearDMO.Where(l => l.MI_Id == data.MI_Id && l.HRMLY_ActiveFlag == true && (l.HRMLY_FromDate <= datetimennow && l.HRMLY_ToDate >= datetimennow)).Distinct().ToList();

                //var dep_id = _IssueContext.MasterEmployee.Single(e => e.MI_Id == data.MI_Id && e.HRME_Id == empid).HRMD_Id;

                if (yearid.Count > 0)
                {
                    lyear_id = yearid.FirstOrDefault().HRMLY_Id;
                }
                //*********************************** Employee List ********************************************// 

                List<EmailDeviationDTO> empployeelist = new List<EmailDeviationDTO>();
                List<EmailDeviationDTO> Tasklist = new List<EmailDeviationDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_DEVIATED_TASK_EMPLOYEE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader =  cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
                            {
                                empployeelist.Add(new EmailDeviationDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"]),
                                    employeename = Convert.ToString(dataReader["EMPNAME"]),
                                    emailids = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                    HRMD_Id = Convert.ToInt64(dataReader["HRMD_Id"]),
                                    deptname = Convert.ToString(dataReader["HRMD_DepartmentName"]),
                                    desname = Convert.ToString(dataReader["HRMDES_DesignationName"]),
                                   
                                });
                            }
                        }
                       


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (empployeelist != null)
                {
                    if (empployeelist.Count > 0)
                    {
                       
                        foreach (var item in empployeelist)
                        {
                          

                            if (item.emailids != "" && item.emailids != null)
                            {

                                string emailccdetails = "";
                                string emailbccdetails = "";

                                var emaildetails = _IssueContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == item.MI_Id && a.HRMD_Id == item.HRMD_Id).ToList();
                                if (emaildetails.Count > 0)
                                {
                                    emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                                    emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                                }
                                else
                                {
                                    emailccdetails = "";
                                    emailbccdetails = "";
                                }

                                var checktemplate = _IssueContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "DEVIATED_TASK_IND"
                                && a.ISES_MailActiveFlag == true).ToList();

                                if (checktemplate.Count == 0)
                                {
                                    data.message = "Email Template not Mapped to the selected Institution";
                                }
                                else
                                {


                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "ISM_DEVIATED_TASKLIST_FOR_EMAIL";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.BigInt)
                                        {
                                            Value = item.MI_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                SqlDbType.VarChar)
                                        {
                                            Value = item.HRME_Id
                                        });

                                        cmd.Parameters.Add(new SqlParameter("@user_Id",
                              SqlDbType.VarChar)
                                        {
                                            Value = data.UserId
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
                                                    Tasklist.Add(new EmailDeviationDTO
                                                    {
                                                        assignedtoe = Convert.ToString(dataReader["assignedtoe"]),
                                                        ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                                        ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                                        ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                                        ISMTPLTA_Status = Convert.ToString(dataReader["ISMTPLTA_Status"]),
                                                        assignedby = Convert.ToString(dataReader["assignedby"]),
                                                        ISMTCRASTO_StartDate = Convert.ToDateTime(dataReader["ISMTCRASTO_StartDate"]),
                                                        ISMTCRASTO_EndDate = Convert.ToDateTime(dataReader["ISMTCRASTO_EndDate"]),
                                                        HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                                        ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                                        effort = Convert.ToString(dataReader["ISMTCRASTO_EffortInHrs"]),
                                                        daydiff = Convert.ToString(dataReader["daydiff"]),

                                                    });
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    string str = "";
                                    if (Tasklist.Count > 0)
                                    {
                                        int rownumber = 0;


                                        for (int j = 0; j < Tasklist.Count; j++)
                                        {
                                            rownumber += 1;
                                            str += "<tr>" +
                                                "<td align=" + "center" + " style=" + ">" + rownumber + "</td> "
                                                //+  "<td align=" + "left" + ">" + Tasklist[j].assignedtoe + "</td>" 
                                                +
                                                "<td align=" + "left" + ">" + Tasklist[j].ISMTCR_TaskNo + "</td>" +
                                                "<td align=" + "left" + ">" + Tasklist[j].ISMTCR_Title + "</td> " +
                                                "<td align=" + "left" + ">" + Tasklist[j].ISMTCR_Desc + "</td> " +
                                                "<td align=" + "left" + ">" + Tasklist[j].HRMP_Name + "</td> " +
                                                "<td align=" + "left" + ">" + Tasklist[j].ISMTCR_BugOREnhancementFlg + "</td> " +
                                                "<td align=" + "left" + "style=" + "word-break: break-word;" + ">" + Tasklist[j].ISMTPLTA_Status + "</td> " +
                                                "<td align=" + "left" + ">" + Tasklist[j].assignedby + "</td> " +
                                                "<td align=" + "left" + ">" + Tasklist[j].ISMTCRASTO_StartDate.Date + "</td> " +
                                                "<td align=" + "left" + ">" + Tasklist[j].ISMTCRASTO_EndDate.Date + "</td> " +
                                               "<td align=" + "left" + ">" + Tasklist[j].daydiff + "</td> " +
                                                "</tr>";


                                        }

                                        string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;

                                        if (templatedetails.Contains("[DeveloperName]"))
                                        {
                                            templatedetails = templatedetails.Replace("[DeveloperName]", item.employeename.ToUpper());
                                        }
                                        if (templatedetails.Contains("[Department]"))
                                        {
                                            templatedetails = templatedetails.Replace("[Department]", item.deptname.ToUpper());
                                        }
                                        if (templatedetails.Contains("[Designation]"))
                                        {
                                            templatedetails = templatedetails.Replace("[Designation]", item.desname.ToUpper());
                                        }



                                        if (templatedetails.Contains("[DATE]"))
                                        {
                                            templatedetails = templatedetails.Replace("[DATE]", DateTime.Now.ToString("dd/MM/yyyy"));
                                        }
                                       
                                        
                                       

                                        templatedetails = templatedetails.Replace("[MSG]", str);

                                        try
                                        {
                                            
                                               
                                               sendmailexamtt(data.MI_Id, item.emailids, templatedetails, emailccdetails, emailbccdetails, "DEVIATED_TASK_IND");
                                           
                                            
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            
                                        }
                                        
                                    }
                                }
                                
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

        public EmailDeviationDTO updatepriority(EmailDeviationDTO data)
        {
            try
            {
              int s=  _Context.Database.ExecuteSqlCommand("ISM_TasksPrioritiesUpdation @p0", data.MI_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public EmailDeviationDTO blockdailyreport(EmailDeviationDTO data)
        {
            try
            {
                int s = _Context.Database.ExecuteSqlCommand("EmployeeMeMoNoticeEndDateChecking");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public string sendmailexamtt(long MI_Id, string mail_id, string url, string emailccdetails, string emailbccdetails,string temp)
        {
            try
            {
               

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == temp
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                Mailmsg = url;

                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _IssueContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

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
                            var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                        if (emailccdetails != null && emailccdetails != "")
                        {
                            string[] ccmaildetails = emailccdetails.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                message.AddCc(c);
                            }
                        }

                        //******************* EMAIL BCC DETAILS ****************//

                      


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
                            clientsmtp.UseDefaultCredentials = false;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(mail_id));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _IssueContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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

                                //******************* EMAIL CC DETAILS ****************//

                                if (emailccdetails != null && emailccdetails != "")
                                {
                                    string[] ccmaildetails = emailccdetails.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        emailMessage.CC.Add(c);
                                    }
                                }

                                //******************* EMAIL BCC DETAILS ****************//

                                //if (emailbccdetails != null && emailbccdetails != "")
                                //{
                                //    string[] bccmaildetails = emailbccdetails.Split(',');

                                //    foreach (var c in bccmaildetails)
                                //    {
                                //        emailMessage.Bcc.Add(c.ToString());
                                //    }
                                //}

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


                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "DEVIATED_TASK_IND" && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _IssueContext.MasterModules.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return "success";

        }

        public EmailDeviationDTO makercheckerverification(EmailDeviationDTO data)
        {
            try
            {
                string str = "";
                List<EmailDeviationDTO> empployeelist = new List<EmailDeviationDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DR_MakerChekerVerification_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                empployeelist.Add(new EmailDeviationDTO
                                {
                                    HeadId = Convert.ToInt64(dataReader["HeadId"]),
                                    HeadName = Convert.ToString(dataReader["HeadName"]),
                                    IndEmpId = Convert.ToInt64(dataReader["IndEmpId"]),
                                    IndEmpName = Convert.ToString(dataReader["IndEmpName"]),
                                    DRDate = Convert.ToString(dataReader["DRDate"]),
                                    DRStatus = Convert.ToString(dataReader["DRStatus"]),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (empployeelist.Count > 0)
                {
                    str += "<table width='100%' border= '1'><tr align='center' width='100%'>MAKER CHECKER REPORT <b>Date : date1 </b></tr> <tr style = 'background-color:cornflowerblue' ><td> TEAM LEAD NAME </td><td> TEAM MEMBER NAME </td><td> STATUS </td></tr> ";

                    for (int i = 0; i < empployeelist.Count; i++)
                    {
                        str += "<tr><td>" + empployeelist[i].HeadName + "</td><td>" + empployeelist[i].IndEmpName + "</td><td>" + empployeelist[i].DRStatus + "</td></tr>";
                    }
                    str += "</table>";
                }
                else
                {
                    str += "\r\n" + "MAKER CHECKER REPORT <b>Date : date1 </b><br />";
                    str += "\r\n" + "There is NO record for report !!!";
                }

                SendEmail("goutamkumar@vapstech.com", "MAKER CHECKER REPORT", str, 17);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public EmailDeviationDTO senddepartmentapproval(EmailDeviationDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                DateTime toatecon = DateTime.Now;
                string confromdate = "";
                string contodate = "";
                fromdatecon = fromdatecon.AddDays(-1);
                toatecon = toatecon.AddDays(-1);
               confromdate = fromdatecon.ToString("yyyy-MM-dd");
               contodate = toatecon.ToString("yyyy-MM-dd");
                string str = "";
                 var userlist = _IssueContext.ISM_User_Employees_MappingDMO.Where(w => w.ISMUSEMM_Order == 1 && w.ISMUSEMM_ActiveFlag == true).Select(r => r.User_Id).Distinct().ToList();
                List<string> ccemail = new List<string>();

                if (userlist.Count>0)
                {

                    for (int f = 0; f < userlist.Count; f++)
                    {
                        string tlname = "";
                        var depheadlist = (from a in _IssueContext.Staff_User_Login
                                           from b in _IssueContext.MasterEmployee
                                           from c in _IssueContext.Multiple_Email_DMO
                                           where a.Id == userlist[f] && a.Emp_Code == b.HRME_Id && b.HRME_Id == c.HRME_Id && c.HRMEM_DeFaultFlag == "default"
                                           select new EmailDeviationDTO
                                           {
                                               HRME_Id=b.HRME_Id,
                                               employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : b.HRME_EmployeeFirstName)
                                                  + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName)
                                                  + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                             ISMMCLT_EmailId  =c.HRMEM_EmailId
                                           }).Distinct().ToList();

                        if (depheadlist.Count>0)
                        {
                            tlname = depheadlist.FirstOrDefault().employeename;

                            if (!ccemail.Contains(depheadlist.FirstOrDefault().ISMMCLT_EmailId))
                            {
                                ccemail.Add(depheadlist.FirstOrDefault().ISMMCLT_EmailId);
                            }
                        }



                        var emplist = _IssueContext.ISM_User_Employees_MappingDMO.Where(w => w.ISMUSEMM_Order == 1 && w.ISMUSEMM_ActiveFlag == true && w.User_Id== userlist[f]).Select(r => r.HRME_Id).Distinct().ToList();

                        if (emplist.Count>0)
                        {
                            var cnt = 0;
                            for (int j = 0; j < emplist.Count; j++)
                            {


                            List<EmailDeviationDTO> empployeelist = new List<EmailDeviationDTO>();
                            using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "ISM_Detailed_DailyReport_Comments_EMAIL";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.VarChar)
                                {
                                    Value = confromdate
                                });
                                cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.VarChar)
                                {
                                    Value = contodate
                                });

                                cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                                {
                                    Value = emplist[j]
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
                                            empployeelist.Add(new EmailDeviationDTO
                                            {
                                                employeename = (dataReader["EMPLOYEENAME"]).ToString(),
                                                DRStatus = (dataReader["StatusFlag"]).ToString(),
                                                DRDate = Convert.ToString(dataReader["DR_DATE"]),
                                                deptname = Convert.ToString(dataReader["DEPTNAME"]),
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }


                            
                                if (empployeelist.Count > 0)
                                {
                                    cnt = cnt + 1;
                                    if (cnt==1)
                                    {
                                        str += "<table width='100%' border= '1'><tr align='center' width='100%'><b>DEPARTMENT HEADNAME : "+  tlname + "</b></tr> <tr style = 'background-color:cornflowerblue' ><td> DEPARTMENT </td><td> TEAM MEMBER NAME </td><td> STATUS </td></tr> ";
                                    }
                                   

                                    for (int i = 0; i < empployeelist.Count; i++)
                                    {
                                        str += "<tr><td>" + empployeelist[i].deptname + "</td><td>" + empployeelist[i].employeename + "</td><td>" + empployeelist[i].DRStatus + "</td></tr>";
                                    }
                                    

                                    if ( j==emplist.Count-1)
                                    {
                                        str += "</table> <br/>";
                                    }
                                }

                            }

                        }


                    }

                    if (str!="")
                    {
                      //  ccemail = new List<string>();
                        senddraprmail(data.MI_Id, data.UserId, "praveenishwar@vapstech.com", "COMDRSTATUS", ccemail, str);
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;

        }


        public EmailDeviationDTO Sendnotassignedtask_Critical(EmailDeviationDTO data)
        {
            List<EmailDeviationDTO> emplist = new List<EmailDeviationDTO>();
            try
            {

                //*********************************** Employee List ********************************************// 

                List<EmailDeviationDTO> empployeelist = new List<EmailDeviationDTO>();
                List<EmailDeviationDTO> Tasklist = new List<EmailDeviationDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_DEPARTMENT_HEAD_NOT_ASSIGN_CRITICAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                                empployeelist.Add(new EmailDeviationDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                    employeename = Convert.ToString(dataReader["EMPNAME"]),
                                    emailids = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                    HRMD_Id = Convert.ToInt64(dataReader["HRMD_Id"]),
                                });
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (empployeelist != null)
                {
                    if (empployeelist.Count > 0)
                    {

                        foreach (var item in empployeelist)
                        {


                            if (item.emailids != "" && item.emailids != null)
                            {

                                string emailccdetails = "";
                                string emailbccdetails = "";

                                var emaildetails = _IssueContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == item.MI_Id && a.HRMD_Id == item.HRMD_Id).ToList();
                                if (emaildetails.Count > 0)
                                {
                                    emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                                    emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                                }
                                else
                                {
                                    emailccdetails = "";
                                    emailbccdetails = "";
                                }

                                var checktemplate = _IssueContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "NOT_ASSIGNED_TASK_CRITICAL"
                                && a.ISES_MailActiveFlag == true).ToList();

                                if (checktemplate.Count == 0)
                                {
                                    data.message = "Email Template not Mapped to the selected Institution";
                                }
                                else
                                {
                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "GET_DEPARTMENTWISE_NOTASSIGNTASK_CRITICAL";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                                 SqlDbType.BigInt)
                                        {
                                            Value = item.HRMD_Id
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
                                                    Tasklist.Add(new EmailDeviationDTO
                                                    {
                                                        MI_Name = Convert.ToString(dataReader["MI_Name"]),
                                                        ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                                        ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                                        ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                                        ISMTPLTA_Status = Convert.ToString(dataReader["ISMTCR_Status"]),
                                                        assignedby = Convert.ToString(dataReader["createdby"]),
                                                        ISMTCR_CreationDate = Convert.ToDateTime(dataReader["ISMTCR_CreationDate"]),
                                                        HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                                        ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                                        ISMMTCAT_TaskCategoryName = Convert.ToString(dataReader["ISMMTCAT_TaskCategoryName"]),
                                                        IVRMM_ModuleName = Convert.ToString(dataReader["IVRMM_ModuleName"]),

                                                        daydiff = Convert.ToString(dataReader["daydiff"]),
                                                    });
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    string stra = "";


                                    if (Tasklist != null && Tasklist.Count > 0)
                                    {
                                        int rownumber = 0;
                                        stra = stra + "<table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Company Name</th>" +
                                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Task No.</th>" +
                                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Tittle </th>" +
                                                    "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Description </th>" +
                                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Created Date </th>" +
                                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Created By </th>"
                                                     +
                                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Module </th>" +
                                                      "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Days </th>" +
                                                     "</tr>";
                                        for (int j = 0; j < Tasklist.Count; j++)
                                        {
                                            rownumber += 1;
                                            stra = stra + "<tr> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:center;font-size:13px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-weight:bold;font-family: sans-serif;'" + " >" + Tasklist[j].MI_Name + "</td>" +
                                                    "<td align=" + "left " + "style=" + "'font-size:13px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + Tasklist[j].ISMTCR_TaskNo + "</td>" +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_Title + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_Desc + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_CreationDate + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].assignedby + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].IVRMM_ModuleName + "</td> " +
                                                     "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].daydiff + "</td> " +
                                                     "</tr>";
                                        }
                                        stra = stra + "</table>";



                                        string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;


                                        templatedetails = templatedetails.Replace("[MSG]", stra);

                                        try
                                        {

                                            sendmailnotassign(data.MI_Id, "NOT_ASSIGNED_TASK_CRITICAL", item.emailids, templatedetails);


                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);

                                        }

                                    }
                                }

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




        public EmailDeviationDTO Sendnotassignedtask(EmailDeviationDTO data)
        {
            List<EmailDeviationDTO> emplist = new List<EmailDeviationDTO>();
            try
            {
                
                //*********************************** Employee List ********************************************// 

                List<EmailDeviationDTO> empployeelist = new List<EmailDeviationDTO>();
                List<EmailDeviationDTO> Tasklist = new List<EmailDeviationDTO>();
                using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_DEPARTMENT_HEAD_NOT_ASSIGN";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                                empployeelist.Add(new EmailDeviationDTO
                                {
                                    HRME_Id = Convert.ToInt64(dataReader["HRME_Id"]),
                                    employeename = Convert.ToString(dataReader["EMPNAME"]),
                                    emailids = Convert.ToString(dataReader["HRMEM_EmailId"]),
                                    HRMD_Id = Convert.ToInt64(dataReader["HRMD_Id"]),
                                });
                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (empployeelist != null)
                {
                    if (empployeelist.Count > 0)
                    {

                        foreach (var item in empployeelist)
                        {


                            if (item.emailids != "" && item.emailids != null)
                            {

                                string emailccdetails = "";
                                string emailbccdetails = "";

                                var emaildetails = _IssueContext.ISM_Email_Department_MappingDMO.Where(a => a.MI_Id == item.MI_Id && a.HRMD_Id == item.HRMD_Id).ToList();
                                if (emaildetails.Count > 0)
                                {
                                    emailccdetails = emaildetails.FirstOrDefault().ISM_Email_CC;
                                    emailbccdetails = emaildetails.FirstOrDefault().ISM_Email_BCC;
                                }
                                else
                                {
                                    emailccdetails = "";
                                    emailbccdetails = "";
                                }

                                var checktemplate = _IssueContext.smsEmailSetting.Where(a => a.MI_Id == data.MI_Id && a.ISES_Template_Name == "NOT_ASSIGNED_TASK"
                                && a.ISES_MailActiveFlag == true).ToList();

                                if (checktemplate.Count == 0)
                                {
                                    data.message = "Email Template not Mapped to the selected Institution";
                                }
                                else
                                {
                                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "GET_DEPARTMENTWISE_NOTASSIGNTASK";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                                 SqlDbType.BigInt)
                                        {
                                            Value = item.HRMD_Id
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
                                                    Tasklist.Add(new EmailDeviationDTO
                                                    {
                                                        MI_Name = Convert.ToString(dataReader["MI_Name"]),
                                                        ISMTCR_TaskNo = Convert.ToString(dataReader["ISMTCR_TaskNo"]),
                                                        ISMTCR_Title = Convert.ToString(dataReader["ISMTCR_Title"]),
                                                        ISMTCR_Desc = Convert.ToString(dataReader["ISMTCR_Desc"]),
                                                        ISMTPLTA_Status = Convert.ToString(dataReader["ISMTCR_Status"]),
                                                        assignedby = Convert.ToString(dataReader["createdby"]),
                                                        ISMTCR_CreationDate = Convert.ToDateTime(dataReader["ISMTCR_CreationDate"]),
                                                        HRMP_Name = Convert.ToString(dataReader["HRMP_Name"]),
                                                        ISMTCR_BugOREnhancementFlg = Convert.ToString(dataReader["ISMTCR_BugOREnhancementFlg"]),
                                                        ISMMTCAT_TaskCategoryName = Convert.ToString(dataReader["ISMMTCAT_TaskCategoryName"]),
                                                        IVRMM_ModuleName = Convert.ToString(dataReader["IVRMM_ModuleName"]),

                                                        daydiff = Convert.ToString(dataReader["daydiff"]),
                                                    });
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    string stra = "";


                                    if (Tasklist != null && Tasklist.Count > 0)
                                    {
                                      int  rownumber = 0;
                                        stra = stra + "<table width='100%' id=" + "'tablepaging'" + " class=" + "'yui'" + " style=" + "'border-collapse: collapse; width: 100%;'" + "><tr>" +
                                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > SL.NO. </th>" +
                                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Company Name</th>" +
                                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Task No.</th>" +
                                                   "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Tittle </th>" +
                                                    "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Description </th>" +
                                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Created Date </th>" +
                                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Created By </th>" 
                                                     +
                                                     "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Module </th>" +
                                                      "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #0078d4;  color: white;'" + " > Days </th>" +
                                                     "</tr>";
                                        for (int j = 0; j < Tasklist.Count; j++)
                                        {
                                            rownumber += 1;
                                            stra = stra + "<tr> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:center;font-size:13px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-weight:bold;font-family: sans-serif;'" + " >" + Tasklist[j].MI_Name + "</td>" +
                                                    "<td align=" + "left " + "style=" + "'font-size:13px;border: 1px solid black;font-weight:bold;font-family: sans-serif;'" + ">" + Tasklist[j].ISMTCR_TaskNo+  "</td>" +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_Title + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_Desc + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].ISMTCR_CreationDate + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].assignedby + "</td> " +
                                                    "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].IVRMM_ModuleName + "</td> " +
                                                     "<td style=" + "'border: 1px solid black; text-align:left;font-size:13px;font-family: sans-serif;'" + " >" + Tasklist[j].daydiff + "</td> " +
                                                     "</tr>";
                                        }
                                        stra = stra + "</table>";
                                        
                                     

                                        string templatedetails = checktemplate.FirstOrDefault().ISES_MailHTMLTemplate;
                                        

                                        templatedetails = templatedetails.Replace("[MSG]", stra);

                                        try
                                        {

                                            sendmailnotassign(data.MI_Id, "NOT_ASSIGNED_TASK", item.emailids, templatedetails);


                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);

                                        }

                                    }
                                }

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
        #region  visitor sms and email
        public string senddraprmail(long MI_Id, long UserID, string Email, string Template,  List<string> ToEmail, string msgstring)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _Context.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_DR_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

                Mailmsg = Mailmsg.Replace("[MSG]", msgstring);
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
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;

                        if (ToEmail.Count > 0)
                        {
                            foreach (var item in ToEmail)
                            {
                                if (item!=Email)
                                {
                                    message.AddCc(item);
                                }
                                
                            }

                        }
                     


                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }


                        if (Email != null && Email != "")
                        {
                            message.AddTo(Email);
                        }

                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

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
                                if (ToEmail.Count > 0)
                                {
                                    foreach (var item in ToEmail)
                                    {
                                        if (item!= Email)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                        
                                    }
                                }


                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }
                                if (Email != null && Email != "")
                                {
                                    emailMessage.To.Add(new MailAddress(Email));
                                   
                                }

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
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
                        //cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = "Visitor Appointment"
                        //});

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
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion

        //EMAIL

        #region   email
        public string sendmailnotassign(long MI_Id, string Template, string emailid, string msgg )
        {
            try
            {
                
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "no template";
                }

                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var institutionName_email = _Context.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "NOT_ASSIGNTASK_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        
                        cmd.Parameters.Add(new SqlParameter("@Template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

                Mailmsg = msgg;
              

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
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();
                    string Attechement = "";
                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        var message = new SendGridMessage();


                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;


                        message.AddTo(emailid);

                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }
                        if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                        {
                            List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                            eevv.Reverse();

                            if (eevv.Count > 0)
                            {
                                foreach (var item in eevv)
                                {
                                    message.AddCc(item);
                                }
                            }
                        }

                        if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                        {
                            List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                            eevv1.Reverse();

                            if (eevv1.Count > 0)
                            {
                                foreach (var item in eevv1)
                                {
                                    message.AddBcc(item);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";

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

                                emailMessage.To.Add(new MailAddress(emailid));



                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }

                                        }
                                    }
                                }

                                if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                                {
                                    List<string> eevv = new List<string>(template[0].ISES_MailCCId.Split(','));
                                    eevv.Reverse();

                                    if (eevv.Count > 0)
                                    {
                                        foreach (var item in eevv)
                                        {
                                            emailMessage.CC.Add(item);
                                        }
                                    }
                                }

                                if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                                {
                                    List<string> eevv1 = new List<string>(template[0].ISES_MailBCCId.Split(','));
                                    eevv1.Reverse();

                                    if (eevv1.Count > 0)
                                    {
                                        foreach (var item in eevv1)
                                        {
                                            emailMessage.Bcc.Add(item);
                                        }
                                    }
                                }


                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }
                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailid
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
                        //cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        //SqlDbType.VarChar)
                        //{
                        //    Value = "Inventory"
                        //});

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
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }
        #endregion


        //END

        public void SendEmail(string mailid, string subject, string body, long id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_Template_Name.Equals("MAKERCHECKER", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Today.AddDays(-1).Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    //if (mailcc != null && mailcc != "")
                    //{
                    //    string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                    //    if (mail_id.Length > 0)
                    //    {
                    //        for (int i = 0; i < mail_id.Length; i++)
                    //        {
                    //            message.AddBcc(mail_id[i]);
                    //        }
                    //    }
                    //}

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);

                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
        }
    }
}
