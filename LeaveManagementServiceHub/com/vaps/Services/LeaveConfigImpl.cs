using LeaveManagementServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.LeaveManagement;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text.RegularExpressions;
using DomainModel.Model;
using SendGrid.Helpers.Mail;
using System.Net;
using System.IO;
using SendGrid;

namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveConfigImpl : LeaveConfigInterface
    {
        private readonly DomainModelMsSqlServerContext _db;
        public LMContext _lmContext;

        public LeaveConfigImpl(LMContext data)
        {
            _lmContext = data;
        }

        public HR_Leave_Policy_Config_DTO save(HR_Leave_Policy_Config_DTO data)
        {
            try
            {
                var query = _lmContext.HR_Leave_Policy_Config_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                if (query.Count > 0)
                {
                    var resultCount = _lmContext.HR_Leave_Policy_Config_DMO.Single(t => t.HRLPC_Id == query.FirstOrDefault().HRLPC_Id);
                    resultCount.HRLPC_AfterCummulativeTime = data.HRLPC_AfterCummulativeTime;
                    resultCount.HRLPC_CummulativeTime = data.HRLPC_CummulativeTime;
                    resultCount.HRLPC_CummulativeTimeFlag = data.HRLPC_CummulativeTimeFlag;
                    resultCount.HRLPC_EarlyOutFlag = data.HRLPC_EarlyOutFlag;
                    resultCount.HRLPC_EarlyOutTime = data.HRLPC_EarlyOutTime;
                    resultCount.HRLPC_IncludeHolidayFlag = data.HRLPC_IncludeHolidayFlag;
                    resultCount.HRLPC_LateInFlag = data.HRLPC_LateInFlag;
                    resultCount.HRLPC_LateInTime = data.HRLPC_LateInTime;
                    resultCount.HRLPC_LateLeaveFlag = data.HRLPC_LateLeaveFlag;
                    resultCount.HRLPC_LateLOPFlag = data.HRLPC_LateLOPFlag;
                    resultCount.HRLPC_LeavePolicyName = data.HRLPC_LeavePolicyName;
                    resultCount.HRLPC_LeavePrefixSuffixFlag = data.HRLPC_LeavePrefixSuffixFlag;
                    resultCount.HRLPC_NoOfLates = data.HRLPC_NoOfLates;
                    resultCount.HRLPC_NoOfLatesCFFlag = data.HRLPC_NoOfLatesCFFlag;
                    resultCount.HRLPC_NoOfLatesFag = data.HRLPC_NoOfLatesFag;
                    resultCount.HRLPC_ServiceName = data.HRLPC_ServiceName;
                    resultCount.HRLPC_SPName = data.HRLPC_SPName;
                    resultCount.MI_Id = data.MI_Id;
                    resultCount.UpdatedDate = DateTime.Now;
                    resultCount.HRLPC_UpdatedBy = data.UserId;
                    resultCount.HRLPC_SpOrGen = data.HRLPC_SpOrGen;
                    resultCount.HRLPC_AbsentLOPFlag = data.HRLPC_AbsentLOPFlag;
                    resultCount.HRLPC_AbsentLeaveFlag = data.HRLPC_AbsentLeaveFlag;

                    _lmContext.Update(resultCount);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
                else
                {
                    HR_Leave_Policy_Config_DMO obj = new HR_Leave_Policy_Config_DMO();
                    obj.HRLPC_AfterCummulativeTime = data.HRLPC_AfterCummulativeTime;
                    obj.HRLPC_CummulativeTime = data.HRLPC_CummulativeTime;
                    obj.HRLPC_CummulativeTimeFlag = data.HRLPC_CummulativeTimeFlag;
                    obj.HRLPC_EarlyOutFlag = data.HRLPC_EarlyOutFlag;
                    obj.HRLPC_EarlyOutTime = data.HRLPC_EarlyOutTime;
                    obj.HRLPC_IncludeHolidayFlag = data.HRLPC_IncludeHolidayFlag;
                    obj.HRLPC_LateInFlag = data.HRLPC_LateInFlag;
                    obj.HRLPC_LateInTime = data.HRLPC_LateInTime;
                    obj.HRLPC_LateLeaveFlag = data.HRLPC_LateLeaveFlag;
                    obj.HRLPC_LateLOPFlag = data.HRLPC_LateLOPFlag;
                    obj.HRLPC_LeavePolicyName = data.HRLPC_LeavePolicyName;
                    obj.HRLPC_LeavePrefixSuffixFlag = data.HRLPC_LeavePrefixSuffixFlag;
                    obj.HRLPC_NoOfLates = data.HRLPC_NoOfLates;
                    obj.HRLPC_NoOfLatesCFFlag = data.HRLPC_NoOfLatesCFFlag;
                    obj.HRLPC_NoOfLatesFag = data.HRLPC_NoOfLatesFag;
                    obj.HRLPC_ServiceName = data.HRLPC_ServiceName;
                    obj.HRLPC_SPName = data.HRLPC_SPName;
                    obj.MI_Id = data.MI_Id;
                    obj.UpdatedDate = DateTime.Now;
                    obj.CreatedDate = DateTime.Now;
                    obj.HRLPC_CreatedBy = data.UserId;
                    obj.HRLPC_UpdatedBy = data.UserId;
                    obj.HRLPC_SpOrGen = data.HRLPC_SpOrGen;
                    obj.HRLPC_AbsentLOPFlag = data.HRLPC_AbsentLOPFlag;
                    obj.HRLPC_AbsentLeaveFlag = data.HRLPC_AbsentLeaveFlag;

                    _lmContext.Add(obj);
                    var flag = _lmContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
               
            }
            catch(Exception ex)
            {
                data.returnval = false;
            }
            return data;
        }

        public HR_Leave_Policy_Config_DTO getSPName(HR_Leave_Policy_Config_DTO data)
        {
            var resultCount = _lmContext.HR_Leave_Policy_Config_DMO.Where(t => t.MI_Id == data.MI_Id).ToList();
            if (resultCount.Count > 0)
            {
                data.config_data = resultCount.ToArray();
            }
          
            return data;
        }

        public async Task<HR_Leave_Policy_Config_DTO> AutoLeaveApproval(HR_Leave_Policy_Config_DTO data)
        {
            try
            {
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VMS_LeaveAutoApproval_Level";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            while (dataReader1.Read())
                            {
                                data.autonextdata.Add(new AUTONEXTDTO
                                {
                                    MI_Id = (long)(dataReader1["MI_Id"]),
                                    HRELAP_Id = (long)(dataReader1["HRELAP_Id"]),
                                    HRME_Id = (long)(dataReader1["HRME_Id"]),
                                    HEAD_HRME_Id = (long)(dataReader1["HEAD_HRME_Id"]),
                                    HRLAON_SanctionLevelNo = (long)(dataReader1["HRLAON_SanctionLevelNo"]),
                                    HRME_EmailId = Convert.ToString(dataReader1["HRME_EmailId"]),
                                    HEAD_HRME_EmailId = Convert.ToString(dataReader1["HEAD_HRME_EmailId"]),
                                    LeaveStatus = Convert.ToString(dataReader1["LeaveStatus"]),
                                });
                            }
                            //while (await dataReader1.ReadAsync())
                            //{
                            //    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            //    for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                            //    {
                            //        dataRow1.Add(
                            //            dataReader1.GetName(iFiled1),
                            //            dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                            //        );
                            //    }
                            //    retObject1.Add((ExpandoObject)dataRow1);
                            //}
                        }
                        data.responsemessage = "Sucess";
                    }
                    catch (Exception ex)
                    {
                        data.responsemessage = "fail";
                        Console.WriteLine(ex.Message);
                    }
                }

                for(int counter = 0; counter < data.autonextdata.Count; counter++)
                {
                    sendmail(data.MI_Id, data.autonextdata[counter].HEAD_HRME_EmailId, "Leave_AutoNextTemplate", data.autonextdata[counter].HEAD_HRME_Id, data.autonextdata[counter].HRME_Id);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<HR_Leave_Policy_Config_DTO> MonthlyAutoLeaveCredit(HR_Leave_Policy_Config_DTO data)
        {
            try
            {
                //One Who worked 20 days in month 1CL and 1 PL is AutoMetically Credit From this Procedure


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EmpMonthWiseClPlLeavesAutoCredit";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject1 = new List<dynamic>();
                        using (var dataReader1 = await cmd.ExecuteReaderAsync())
                        {
                            //while (dataReader1.Read())
                            //{
                            //    data.autonextdata.Add(new AUTONEXTDTO
                            //    {
                            //        MI_Id = (long)(dataReader1["MI_Id"]),
                            //        HRELAP_Id = (long)(dataReader1["HRELAP_Id"]),
                            //        HRME_Id = (long)(dataReader1["HRME_Id"]),
                            //        HEAD_HRME_Id = (long)(dataReader1["HEAD_HRME_Id"]),
                            //        HRLAON_SanctionLevelNo = (long)(dataReader1["HRLAON_SanctionLevelNo"]),
                            //        HRME_EmailId = Convert.ToString(dataReader1["HRME_EmailId"]),
                            //        HEAD_HRME_EmailId = Convert.ToString(dataReader1["HEAD_HRME_EmailId"]),
                            //        LeaveStatus = Convert.ToString(dataReader1["LeaveStatus"]),
                            //    });
                            //}
                            while (await dataReader1.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1]
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.responsemessage = "Sucess";
                    }
                    catch (Exception ex)
                    {
                        data.responsemessage = "fail";
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public HR_Leave_Policy_Config_DTO LeaveRejectafterUnAuthorizedLeave(HR_Leave_Policy_Config_DTO data)
        {
            try
            {
                var listofleaveapproved = _lmContext.HR_Emp_Leave_Trans_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRELT_Status == "Approved" && t.HRELT_LeaveReason != "LWP Cumulative Report").ToList();
                using (var cmd = _lmContext.Database.BeginTransaction())
                {
                    for (int counter = 0; counter < listofleaveapproved.Count; counter++)
                    {
                        try
                        {
                            DateTime? nextday = (from a in _lmContext.holidaydate
                                           from b in _lmContext.holidayWorkingDayType
                                           where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && b.FOHTWD_HolidayFlag == false && b.FOHWDT_ActiveFlg == true && a.FOHWDT_Id == b.FOHWDT_Id && a.FOMHWDD_FromDate > listofleaveapproved[counter].HRELT_ToDate)
                                           select a.FOMHWDD_FromDate).FirstOrDefault();

                            var attlogs = _lmContext.FO_Emp_Punch.Where(t => t.HRME_Id == listofleaveapproved[counter].HRME_Id && t.FOEP_PunchDate == nextday).ToList();
                            if(attlogs.Count == 0)
                            {
                                var listofleaveupdate = _lmContext.HR_Emp_Leave_Trans_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRELT_Status == "Approved" && t.HRELT_LeaveReason != "LWP Cumulative Report" && t.HRELT_Id == listofleaveapproved[counter].HRELT_Id).FirstOrDefault();

                                listofleaveupdate.HRELT_Status = "Rejected";
                                listofleaveupdate.HRELT_LeaveReason = "AutoReject";
                                _lmContext.Update(listofleaveupdate);
                                _lmContext.SaveChanges();
                            }
                        }
                        catch
                        {
                            cmd.Rollback();
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

        public string sendmail(long MI_Id, string Email, string Template, long HEAD_HRME_Id, long HRME_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var headname = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == HEAD_HRME_Id).Select(t => new { t.HRME_EmployeeFirstName, t.HRME_EmployeeMiddleName, t.HRME_EmployeeLastName }).FirstOrDefault();
                string headnamestring = headname.HRME_EmployeeFirstName + " " + (headname.HRME_EmployeeMiddleName == null ? "" : headname.HRME_EmployeeMiddleName) + " " + (headname.HRME_EmployeeLastName == null ? "" : headname.HRME_EmployeeLastName);

                var empname = _lmContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == HRME_Id).Select(t => new { t.HRME_EmployeeFirstName, t.HRME_EmployeeMiddleName, t.HRME_EmployeeLastName }).FirstOrDefault();
                string empnamestring = headname.HRME_EmployeeFirstName + " " + (headname.HRME_EmployeeMiddleName == null ? "" : headname.HRME_EmployeeMiddleName) + " " + (headname.HRME_EmployeeLastName == null ? "" : headname.HRME_EmployeeLastName);
                //var Paramaeters = _FOContext.SMS_MAIL_SAVED_PARAMETER  _DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                //var ParamaetersName = _FOContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                Mailmsg.Replace("[NAME]", headnamestring);
                Mailmsg.Replace("[APPLIEDNAME]", empnamestring);
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
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
                    string mailcc = "";
                    string mailbcc = "";
                    alldetails[0].IVRM_mailcc = "";
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
                    if (Email == null) { Email = "hr@vapstech.com"; }
                    message.AddTo(Email);

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
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
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