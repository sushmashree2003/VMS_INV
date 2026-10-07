using CommonLibrary;
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
using System.Text;


namespace IssueManager.com.vaps.Services
{
    public class ISM_PrinterStatisticsReportImpl : Interfaces.ISM_PrinterStatisticsReportInterface
    {
        public IssueManagerContext _IssueContext;


        public ISM_PrinterStatisticsReportImpl(IssueManagerContext isc)
        {





            _IssueContext = isc;

        }

        public async Task<ISM_PrinterStatisticsDTO> getdetails(ISM_PrinterStatisticsDTO data)
        {
            try
            {



                data.get_department = (from a in _IssueContext.HR_Master_Department

                                       where (a.MI_Id == data.MI_Id && a.HRMD_ActiveFlag == true)
                                       select new ISM_PrinterStatisticsDTO
                                       {
                                           HRMD_Id = a.HRMD_Id,
                                           HRMD_DepartmentName = a.HRMD_DepartmentName
                                       }).Distinct().ToArray();
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == data.MI_Id && t.Id == data.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                long? dept_id = 0;
                if (hrmeDepartment_id.Count > 0)
                {
                    dept_id = hrmeDepartment_id.FirstOrDefault().HRMD_Id;
                }
                data.dept_Id = Convert.ToInt32(dept_id);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<ISM_PrinterStatisticsDTO> get_report(ISM_PrinterStatisticsDTO data)
        {
            try
            {
                string FromDate = data.startdate.Value.ToString("yyyy-MM-dd");
                string ToDate = data.enddate.Value.ToString("yyyy-MM-dd");

                if (data.flag == 1)
                {

                    if (data.HRMD_Id == 0)
                    {
                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id   && (a.VMSPOD_Type == data.VMSPOD_Type_One || a.VMSPOD_Type == data.VMSPOD_Type_Two) && a.VMSPOD_FromDate.Value.Date >= data.startdate.Value.Date && a.VMSPOD_ToDate.Value.Date <= data.enddate.Value.Date
                                                 && a.VMSPOD_ActiveFlag == true && c.VMSMPR_Id==a.VMSMPR_Id)
                                                 select new ISM_PrinterStatisticsDTO
                                                 {
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                     VMSPOD_PODate = a.VMSPOD_PODate,
                                                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                     VMSPOD_Type = a.VMSPOD_Type,
                                                     VMSPOD_Id = a.VMSPOD_Id,
                                                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                     VMSMPR_Name=c.VMSMPR_Name




                                                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();

                    }
                    else
                    {
                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id   && (a.VMSPOD_Type == data.VMSPOD_Type_One || a.VMSPOD_Type == data.VMSPOD_Type_Two) && a.VMSPOD_FromDate.Value.Date >= data.startdate.Value.Date && a.VMSPOD_ToDate.Value.Date <= data.enddate.Value.Date && a.HRMD_Id == data.HRMD_Id && b.HRMD_Id == data.HRMD_Id
                                                 && a.VMSPOD_ActiveFlag == true && c.VMSMPR_Id == a.VMSMPR_Id)
                                                 select new ISM_PrinterStatisticsDTO
                                                 {
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                     VMSPOD_PODate = a.VMSPOD_PODate,
                                                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                     VMSPOD_Type = a.VMSPOD_Type,
                                                     VMSPOD_Id = a.VMSPOD_Id,
                                                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                     VMSMPR_Name = c.VMSMPR_Name




                                                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();
                    }
                }
                else if (data.flag == 2)
                {
                    if (data.HRMD_Id == 0)
                    {
                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id   && a.VMSPOD_Type == data.VMSPOD_Type_Two && a.VMSPOD_FromDate.Value.Date >= data.startdate.Value.Date && a.VMSPOD_ToDate.Value.Date <= data.enddate.Value.Date 
                                                 && a.VMSPOD_ActiveFlag == true && c.VMSMPR_Id == a.VMSMPR_Id)
                                                 select new ISM_PrinterStatisticsDTO
                                                 {
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                     VMSPOD_PODate = a.VMSPOD_PODate,
                                                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                     VMSPOD_Type = a.VMSPOD_Type,
                                                     VMSPOD_Id = a.VMSPOD_Id,
                                                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                     VMSMPR_Name = c.VMSMPR_Name




                                                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();
                    }
                    else
                    {
                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id   && a.VMSPOD_Type == data.VMSPOD_Type_Two && a.VMSPOD_FromDate.Value.Date >= data.startdate.Value.Date && a.VMSPOD_ToDate.Value.Date <= data.enddate.Value.Date && a.HRMD_Id == data.HRMD_Id && b.HRMD_Id == data.HRMD_Id 
                                                 && a.VMSPOD_ActiveFlag == true && c.VMSMPR_Id == a.VMSMPR_Id)
                                                 select new ISM_PrinterStatisticsDTO
                                                 {
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                     VMSPOD_PODate = a.VMSPOD_PODate,
                                                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                     VMSPOD_Type = a.VMSPOD_Type,
                                                     VMSPOD_Id = a.VMSPOD_Id,
                                                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                     VMSMPR_Name = c.VMSMPR_Name




                                                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();
                    }

                }
                else if (data.flag == 3)
                {
                    if (data.HRMD_Id == 0)
                    {
                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id   && a.VMSPOD_Type == data.VMSPOD_Type_One && a.VMSPOD_FromDate.Value.Date >= data.startdate.Value.Date && a.VMSPOD_ToDate.Value.Date <= data.enddate.Value.Date
                                                 && a.VMSPOD_ActiveFlag == true && c.VMSMPR_Id == a.VMSMPR_Id)
                                                 select new ISM_PrinterStatisticsDTO
                                                 {
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                     VMSPOD_PODate = a.VMSPOD_PODate,
                                                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                     VMSPOD_Type = a.VMSPOD_Type,
                                                     VMSPOD_Id = a.VMSPOD_Id,
                                                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                     VMSMPR_Name = c.VMSMPR_Name

                                                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();
                    }
                    else
                    {
                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id   && a.VMSPOD_Type == data.VMSPOD_Type_One && a.VMSPOD_FromDate.Value.Date >= data.startdate.Value.Date && a.VMSPOD_ToDate.Value.Date <= data.enddate.Value.Date && a.HRMD_Id == data.HRMD_Id && b.HRMD_Id == data.HRMD_Id 
                                                 && a.VMSPOD_ActiveFlag == true && c.VMSMPR_Id == a.VMSMPR_Id)
                                                 select new ISM_PrinterStatisticsDTO
                                                 {
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                     VMSPOD_PODate = a.VMSPOD_PODate,
                                                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                     VMSPOD_Type = a.VMSPOD_Type,
                                                     VMSPOD_Id = a.VMSPOD_Id,
                                                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                     VMSMPR_Name = c.VMSMPR_Name




                                                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();
                    }
                }
                else
                {
                    if (data.HRMD_Id == 0)
                    {

                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id   && a.VMSPOD_FromDate.Value.Date >= data.startdate.Value.Date && a.VMSPOD_ToDate.Value.Date <= data.enddate.Value.Date 
                                                 && a.VMSPOD_ActiveFlag == true && c.VMSMPR_Id == a.VMSMPR_Id)
                                                 select new ISM_PrinterStatisticsDTO
                                                 {
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                     VMSPOD_PODate = a.VMSPOD_PODate,
                                                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                     VMSPOD_Type = a.VMSPOD_Type,
                                                     VMSPOD_Id = a.VMSPOD_Id,
                                                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                     VMSMPR_Name = c.VMSMPR_Name




                                                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();
                    }
                    else
                    {


                        data.get_printdetails = (from a in _IssueContext.ISM_PrinterStatisticsDMO
                                                 from b in _IssueContext.HR_Master_Department
                                                 from c in _IssueContext.PrinterDMO
                                                 where (a.HRMD_Id == b.HRMD_Id && a.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id   && a.VMSPOD_FromDate.Value.Date >= data.startdate.Value.Date && a.VMSPOD_ToDate.Value.Date <= data.enddate.Value.Date && a.HRMD_Id == data.HRMD_Id && b.HRMD_Id == data.HRMD_Id
                                                 && a.VMSPOD_ActiveFlag == true && c.VMSMPR_Id == a.VMSMPR_Id)
                                                 select new ISM_PrinterStatisticsDTO
                                                 {
                                                     HRMD_Id = a.HRMD_Id,
                                                     HRMD_DepartmentName = b.HRMD_DepartmentName,
                                                     VMSPOD_PODate = a.VMSPOD_PODate,
                                                     VMSPOD_Remarks = a.VMSPOD_Remarks,
                                                     VMSPOD_NoOfPages = a.VMSPOD_NoOfPages,
                                                     VMSPOD_Type = a.VMSPOD_Type,
                                                     VMSPOD_Id = a.VMSPOD_Id,
                                                     VMSPOD_ActiveFlag = a.VMSPOD_ActiveFlag,
                                                     VMSMPR_Name = c.VMSMPR_Name




                                                 }).Distinct().OrderByDescending(a => a.VMSPOD_Id).ToArray();
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<ISM_PrinterStatisticsDTO> sendmail(ISM_PrinterStatisticsDTO dto)
        {
            try
            {
                string hrmdid = "0";
                foreach(var i in dto.hrmdids)
                {
                    hrmdid = hrmdid+"," + i.HRMD_Id;
                }
               
                var hrmEempid = _IssueContext.Staff_User_Login.Where(t => t.MI_Id == dto.MI_Id && t.Id == dto.UserId).Distinct().ToList();
                long empid = 0;
                if (hrmEempid.Count > 0)
                {
                    empid = hrmEempid.FirstOrDefault().Emp_Code;
                }
                var hrmeDepartment_id = _IssueContext.MasterEmployee.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id == empid && t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).Distinct().ToList();
                string emailid = "";
                if (hrmeDepartment_id.Count > 0)
                {
                    emailid = hrmeDepartment_id.FirstOrDefault().HRME_EmailId;
                }
                string emailccdetails = "";
                string heademail = "";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _IssueContext.smsEmailSetting.Where(e => e.ISES_Template_Name == "PrinterMail" && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    dto.retrunMsg = "Email Template not Mapped to the selected Institution";
                    return dto;
                }
                var institutionName = _IssueContext.Institution.Where(i => i.MI_Id == dto.MI_Id).ToList();
                var Paramaeters = _IssueContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == dto.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _IssueContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                //TEMPLATE EDITING
                Mailmsg = dto.Templatedata;
                //TEMPLATE EDITING

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _IssueContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subjectname = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";
           
                    List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_DepartmentHeadmultiple";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                        SqlDbType.VarChar)
                        {
                            Value = hrmdid
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
                                    dtonewtemp.Add(new ISM_PlannerReportsDTO
                                    {
                                        Tomail = Convert.ToString(dataReader["Head"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    if (dtonewtemp.Count() > 0)
                    {

                        for (int j = 0; j < dtonewtemp.Count; j++)

                        {
                            if (j == 0)
                            {
                                heademail = dtonewtemp[j].Tomail;
                            }
                            else
                            {
                                heademail = heademail + "," + dtonewtemp[j].Tomail;
                            }
                        }
                    }


                    if (heademail != "")
                    {
                        emailccdetails = heademail;
                    }


                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subjectname;
                   // message.AddTo(template[0].ISES_MailCCId);

                    if (template.Count > 0)
                    {
                        string[] ccmaildetails = template[0].ISES_MailCCId.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddTo(c);
                        }
                    }
                   // message.AddCc(emailccdetails);
                    if (emailccdetails != null && emailccdetails != "")
                    {
                        string[] ccmaildetails = emailccdetails.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }

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
                    client.SendEmailAsync(message).Wait();

                    using (var cmd = _IssueContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _IssueContext.smsEmailSetting.Where(e => e.ISES_Template_Name == "PrinterMail" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _IssueContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        // var modulename = _IssueContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = template[0].ISES_MailCCId
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = "Printer Statistics"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
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
                            dto.retrunMsg = ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dto.retrunMsg = ex.Message;
            }

            dto.retrunMsg = "success";

            return dto;

        }



    }
}
