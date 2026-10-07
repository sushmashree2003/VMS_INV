using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
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

namespace HRMSServiceHub.com.vaps.Services
{
    public class SalaryAdvanceApprovalIMPL : Interfaces.SalaryAdvanceApprovalInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public SalaryAdvanceApprovalIMPL(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Emp_SalaryAdvanceDTO getBasicData(HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.retrunMsg = "";
            try
            {

                dto = GetAllDropdownAnddtotableDetails(dto);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public HR_Emp_SalaryAdvanceDTO GetAllDropdownAnddtotableDetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            try
            {

                var HRME_Id = _HRMSContext.Staff_User_Login.Where(t => t.Id == dto.LogInUserId).Select(t => t.Emp_Code).FirstOrDefault();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Salary_Adavance_Approval";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.LogInUserId
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                        dtoReader.GetName(iFiled),
                                        dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.empadvaList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Salary_Advance_Approval_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                    SqlDbType.BigInt)
                    {
                        Value = dto.LogInUserId
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_Emp_SalaryAdvanceDTO empdetails(HR_Emp_SalaryAdvanceDTO dto)
        {
            try
            {

                var ApprovalHRME_Id = _HRMSContext.Staff_User_Login.Where(t => t.Id == dto.LogInUserId).Select(t => t.Emp_Code).FirstOrDefault();

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Employee_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.griddisplay = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Salary_Advance_Approval_Grid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = ApprovalHRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRESA_Id", SqlDbType.BigInt)                    {                        Value = dto.HRESA_Id                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(
                                    dtoReader.GetName(iFiled),
                                    dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);

            }
            return dto;

        }
        public HR_Emp_SalaryAdvanceDTO approvedrecord(HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO conobj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);


                var currentlevel = "";                var CurrentApprovalMail = "";                var NextLevel = "";                var NextlevelApprovalMail = "";                var HRMDES_DesignationName = "";                var hremail = "";                var ccemail = "";                string toemailid = "";                string employeeEmail = "";                string FinalFlg = "";                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_SalaryAdvance_SactionLevel";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)                    {                        Value = dto.LogInUserId                    });
                    cmd.Parameters.Add(new SqlParameter("@HRESA_Id", SqlDbType.BigInt)                    {                        Value = dto.HRESA_Id                    });


                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]                                    );                                }


                                currentlevel = dataReader["currentlevel"].ToString();
                                CurrentApprovalMail = dataReader["CurrentApprovalMail"].ToString();
                                NextLevel = dataReader["NextLevel"].ToString();
                                NextlevelApprovalMail = dataReader["NextlevelApprovalMail"].ToString();
                                HRMDES_DesignationName = dataReader["HRMDES_DesignationName"].ToString();                                hremail = dataReader["HRMail"].ToString();                                employeeEmail = dataReader["EmployeeEmail"].ToString();                                FinalFlg = dataReader["HRPAON_FinalFlg"].ToString();





                            }                        }
                    }

                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }

                }

                string emailid = "";
                string Template = "";
                var ccemailid = "";
                var approvalstatus = dto.HRESA_AdvStatus;

                if (dto.HRESA_AdvStatus == "Approved")
                {
                    if (Convert.ToBoolean(FinalFlg) == true)
                    {
                        dto.HRESA_AdvStatus = "Sanctioned";
                        dto.HRESA_SanctinedAmount = dto.HRESA_SanctinedAmount;

                        emailid = _HRMSContext.Emp_Email_Id.Where(p => p.HRME_Id == dto.HRME_Id && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        Template = "Salary_Advance_Approval";

                        if (emailid != null && emailid.Length > 0)
                        {                       
                            sendEmail(dto.MI_Id, Template, dto.HRESA_Id, emailid, ccemailid);
                        }
                    }
                    else
                    {
                        dto.HRESA_AdvStatus = "Partial Approved";
                        dto.HRESA_SanctinedAmount = dto.HRESA_SanctinedAmount;
                    }
                }

                var HRESA_Id = _HRMSContext.HR_Emp_SalaryAdvance.Single(t => t.HRESA_Id == dto.HRESA_Id);

                if (dto.HRESA_Id > 0)
                {

                    HR_Emp_SalaryAdvance_ApprovalDMO advapp = new HR_Emp_SalaryAdvance_ApprovalDMO();

                    advapp.HRESA_Id = dto.HRESA_Id;
                    advapp.IVRMUL_Id = dto.LogInUserId;
                    advapp.HRESAA_Remarks = dto.HRESAA_Remarks;
                    advapp.HRESAA_Date = DateTime.Now;
                    advapp.HRESAA_SanctionLevel = Convert.ToInt32(currentlevel);
                    advapp.HRESAA_ActiveFlag = true;
                    advapp.CreatedDate = DateTime.Now;
                    advapp.UpdatedDate = DateTime.Now;
                    advapp.HRESAA_CreatedBy = dto.LogInUserId;
                    advapp.HRESAA_UpdatedBy = dto.LogInUserId;
                    advapp.HRESAA_Status = approvalstatus;
                    advapp.HRESAA_Amount = dto.HRESA_SanctinedAmount;
                    _HRMSContext.Add(advapp);
                    _HRMSContext.SaveChanges();

                    var result = _HRMSContext.HR_Emp_SalaryAdvance.Single(t => t.HRESA_Id == dto.HRESA_Id);
                    result.UpdatedDate = DateTime.Now;
                    result.HRESA_UpdatedBy = dto.LogInUserId;
                    result.HRESA_AdvStatus = dto.HRESA_AdvStatus;
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        dto.retrunMsg = "Update";
                    }
                    else
                    {
                        dto.retrunMsg = "false";
                    }

                }



                dto = GetAllDropdownAnddtotableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public string sendEmail(long MI_Id, string Template, long HRESA_Id, string email, string ccemail)        {            try            {                Dictionary<string, string> val = new Dictionary<string, string>();                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template                && e.ISES_MailActiveFlag == true).ToList();                if (template.Count == 0)                {                    return "Email Template not Mapped to the selected Institution";                }                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Resultsms = Mailcontent;                string result = Mailmsg;                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "HR_Email_Prametars_Replace";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",                  SqlDbType.VarChar)                    {                        Value = HRESA_Id                    });                    cmd.Parameters.Add(new SqlParameter("@Type",                  SqlDbType.VarChar)                    {                        Value = Template                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                    var datatype = dataReader.GetFieldType(iFiled);                                    if (datatype.Name == "DateTime")                                    {                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else if (datatype.Name == "Decimal")                                    {                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else                                    {                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());                                    }                                }                            }                        }                    }                    catch (Exception ex)                    {                        return ex.Message;                    }                }                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailmsg = result;                        }                    }                }                Mailmsg = result;                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {


                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailcontent = Resultsms;                        }                    }                }                Mailcontent = Resultsms;                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();                string Attechement = "";                if (alldetails.Count > 0)                {                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);                    string Subject = template[0].ISES_MailSubject.ToString();                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();                    string mailcc = "";                    string mailbcc = "";

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")                    {                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();                    }                    var message = new SendGridMessage();                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);                    message.Subject = Subject;                    message.AddTo(email);


                    if (Attechement.Equals("1"))                    {                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();                        if (img.Count > 0)                        {                            for (int i = 0; i < img.Count; i++)                            {                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;                                Stream stream = response.GetResponseStream();                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);                            }                        }                    }
                    //****************** EMAIL CC DETAILS ***************//

                    if (ccemail != null && ccemail != "")                    {                        string[] ccmaildetails = ccemail.Split(',');                        foreach (var c in ccmaildetails)                        {                            message.AddCc(c);                        }                    }                   

                    //****************** EMAIL BCC DETAILS ***************//




                    if (mailcc != null && mailcc != "")                    {
                        //message.AddCc(mailcc);
                    }                    if (mailbcc != null && mailbcc != "")                    {                        message.AddBcc(mailbcc);                    }                    message.HtmlContent = Mailmsg;                    var client = new SendGridClient(sengridkey);                    try                    {                        client.SendEmailAsync(message).Wait();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())                    {                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();                        cmd.CommandText = "IVRM_Email_Outgoing";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@EmailId",                            SqlDbType.NVarChar)                        {                            Value = email                        });                        cmd.Parameters.Add(new SqlParameter("@Message",                           SqlDbType.NVarChar)                        {                            Value = Mailcontent                        });                        cmd.Parameters.Add(new SqlParameter("@module",                        SqlDbType.VarChar)                        {                            Value = modulename[0]                        });                        cmd.Parameters.Add(new SqlParameter("@MI_Id",                       SqlDbType.BigInt)                        {                            Value = MI_Id                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        try                        {                            using (var dataReader = cmd.ExecuteReader())                            {                            }                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return "success";        }

        //------------------ Advance Approval Report------------------------------
        public HR_Emp_SalaryAdvanceDTO Onload(HR_Emp_SalaryAdvanceDTO dto)
        {
            try
            {


                dto.fillyear = (from a in _HRMSContext.HR_MasterLeaveYear
                                where (a.MI_Id == dto.MI_Id && a.HRMLY_ActiveFlag == true)
                                select new HR_Master_LeaveYearDTO
                                {
                                    HRMLY_Id = a.HRMLY_Id,
                                    HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                    HRMLY_LeaveYearOrder = a.HRMLY_LeaveYearOrder
                                }).Distinct().OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();


                dto.fillmonth = _HRMSContext.Month.Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();

                var employees = _HRMSContext.HR_Emp_SalaryAdvance.Where(R => R.HRESA_ActiveFlag == true).Distinct().ToList();
                List<long> HRME_Id = new List<long>();
                if (employees.Count > 0)
                {
                    foreach (var d in employees)
                    {
                        HRME_Id.Add(d.HRME_Id);
                    }
                }
                dto.employees = _HRMSContext.MasterEmployee.Where(R => R.HRME_ActiveFlag == true && HRME_Id.Contains(R.HRME_Id) && R.MI_Id == dto.MI_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public HR_Emp_SalaryAdvanceDTO getapprovalreport(HR_Emp_SalaryAdvanceDTO dto)
        {
            try
            {
                var from_date = dto.Fromdate.Value.Date.ToString("yyyy-MM-dd");
                var To_date = dto.Todate.Value.Date.ToString("yyyy-MM-dd");


                List<string> IVRM_Month_Name = new List<string>();                for (var i = 0; i < dto.MONTHID.Length; i++)                {                    IVRM_Month_Name.Add(dto.MONTHID[i].IVRM_Month_Name);                }                string monthid = string.Join(",", IVRM_Month_Name);


                List<long> HRME_Id = new List<long>();

                for (var i = 0; i < dto.HRMEID.Length; i++)
                {
                    HRME_Id.Add(dto.HRMEID[i].HRME_Id);
                }
                string employeeid = string.Join(",", HRME_Id);

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Advance_APPROVAL_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.VarChar) { Value = employeeid });
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar) { Value = dto.yearid });
                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Id", SqlDbType.VarChar) { Value = monthid });
                    cmd.Parameters.Add(new SqlParameter("@From_Date", SqlDbType.VarChar) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.VarChar) { Value = To_date });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = dto.rdbbutton });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.VarChar) { Value = dto.Status_Type });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dtoReader = cmd.ExecuteReader())
                        {
                            while (dtoReader.Read())
                            {
                                var dtoRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dtoReader.FieldCount; iFiled++)
                                {
                                    dtoRow.Add(dtoReader.GetName(iFiled), dtoReader.IsDBNull(iFiled) ? null : dtoReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dtoRow);
                            }
                        }
                        dto.approvalReport = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}




