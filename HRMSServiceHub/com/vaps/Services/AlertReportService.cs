using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class AlertReportService : Interfaces.AlertReportInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public AlertReportService(HRMSContext HRMSContext, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _HRMSContext = HRMSContext;
            _Context = MsSqlServerContext;
        }

        public async Task<MasterEmployeeDTO> getAlertReport (MasterEmployeeDTO dto)
        {
            List<MasterEmployeeDTO> employeeDetails = new List<MasterEmployeeDTO>();
            double dbldays = 0;
            double aleryday = 0;
            DateTime crntdate = DateTime.Today;
            var departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.HRMD_ActiveFlag == true).ToArray();
            var designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.HRMDES_ActiveFlag == true).ToArray();
            var alertdays = dto.MI_Id; // _HRMSContext.HR_Configuration.Where(t => t.MI_Id == dto.MI_Id).Select(t => t.HRC_AlertDay).FirstOrDefault();
            aleryday = Convert.ToDouble(alertdays);
            try
            {
                if (dto.Type.Equals("Retirement"))
                {
                    var empDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();
                    for (int i = 0; i < empDetails.Length; i++)
                    {
                        dbldays = (DateTime.Today - Convert.ToDateTime(empDetails[i].HRME_DOB).AddYears(60)).TotalDays;
                        if (dbldays < aleryday)
                        {
                            MasterEmployeeDTO ss = new MasterEmployeeDTO();
                            ss.HRME_EmployeeFirstName = empDetails[i].HRME_EmployeeFirstName;
                            ss.HRME_EmployeeMiddleName = empDetails[i].HRME_EmployeeMiddleName;
                            ss.HRME_EmployeeLastName = empDetails[i].HRME_EmployeeLastName;
                            ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                            ss.HRME_DOB = empDetails[i].HRME_DOB;
                            ss.HRME_SalaryType = departmentlist.Where(t => t.HRMD_Id == empDetails[i].HRMD_Id).Select(t => t.HRMD_DepartmentName).FirstOrDefault();
                            ss.HRME_UINumber = designationlist.Where(t => t.HRMDES_Id == empDetails[i].HRMDES_Id).Select(t => t.HRMDES_DesignationName).FirstOrDefault();
                            employeeDetails.Add(ss);
                        }
                    }
                    dto.employeedetailList = employeeDetails.ToArray();
                }
                else if (dto.Type.Equals("Increment"))
                {
                    var empDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false).ToArray();
                    for (int i = 0; i < empDetails.Length; i++)
                    {
                        dbldays = (DateTime.Today - Convert.ToDateTime(empDetails[i].HRME_DOJ).AddYears(1)).TotalDays;
                        if (dbldays < aleryday)
                        {
                            MasterEmployeeDTO ss = new MasterEmployeeDTO();
                            ss.HRME_EmployeeFirstName = empDetails[i].HRME_EmployeeFirstName;
                            ss.HRME_EmployeeMiddleName = empDetails[i].HRME_EmployeeMiddleName;
                            ss.HRME_EmployeeLastName = empDetails[i].HRME_EmployeeLastName;
                            ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                            ss.HRME_DOB = empDetails[i].HRME_DOB;
                            ss.HRME_SalaryType = departmentlist.Where(t => t.HRMD_Id == empDetails[i].HRMD_Id).Select(t => t.HRMD_DepartmentName).FirstOrDefault();
                            ss.HRME_UINumber = designationlist.Where(t => t.HRMDES_Id == empDetails[i].HRMDES_Id).Select(t => t.HRMDES_DesignationName).FirstOrDefault();
                            employeeDetails.Add(ss);
                        }
                    }
                    dto.employeedetailList = employeeDetails.ToArray();
                }
                else if (dto.Type.Equals("Probation"))
                {
                    var empDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && (DateTime.Today - Convert.ToDateTime(t.HRME_DOJ).AddMonths(6)).TotalDays == aleryday).ToArray();
                    for (int i = 0; i < empDetails.Length; i++)
                    {
                        MasterEmployeeDTO ss = new MasterEmployeeDTO();
                        ss.HRME_EmployeeFirstName = empDetails[i].HRME_EmployeeFirstName;
                        ss.HRME_EmployeeMiddleName = empDetails[i].HRME_EmployeeMiddleName;
                        ss.HRME_EmployeeLastName = empDetails[i].HRME_EmployeeLastName;
                        ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                        ss.HRME_DOB = empDetails[i].HRME_DOB;
                        ss.HRME_SalaryType = departmentlist.Where(t => t.HRMD_Id == empDetails[i].HRMD_Id).Select(t => t.HRMD_DepartmentName).FirstOrDefault();
                        ss.HRME_UINumber = designationlist.Where(t => t.HRMDES_Id == empDetails[i].HRMDES_Id).Select(t => t.HRMDES_DesignationName).FirstOrDefault();
                        employeeDetails.Add(ss);
                    }
                    dto.employeedetailList = employeeDetails.ToArray();
                }
                else if (dto.Type.Equals("Gratuity"))
                {
                    var empDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && (DateTime.Today - Convert.ToDateTime(t.HRME_DOJ).AddYears(5)).TotalDays == aleryday).ToArray();
                    for (int i = 0; i < empDetails.Length; i++)
                    {
                        MasterEmployeeDTO ss = new MasterEmployeeDTO();
                        ss.HRME_EmployeeFirstName = empDetails[i].HRME_EmployeeFirstName;
                        ss.HRME_EmployeeMiddleName = empDetails[i].HRME_EmployeeMiddleName;
                        ss.HRME_EmployeeLastName = empDetails[i].HRME_EmployeeLastName;
                        ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                        ss.HRME_DOB = empDetails[i].HRME_DOB;
                        ss.HRME_SalaryType = departmentlist.Where(t => t.HRMD_Id == empDetails[i].HRMD_Id).Select(t => t.HRMD_DepartmentName).FirstOrDefault();
                        ss.HRME_UINumber = designationlist.Where(t => t.HRMDES_Id == empDetails[i].HRMDES_Id).Select(t => t.HRMDES_DesignationName).FirstOrDefault();
                        employeeDetails.Add(ss);
                    }
                    dto.employeedetailList = employeeDetails.ToArray();
                }
                else if (dto.Type.Equals("TrainingEvaluation"))
                {
                    var empDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && (DateTime.Today - Convert.ToDateTime(t.HRME_DOJ).AddMonths(6)).TotalDays == aleryday).ToArray();
                    for (int i = 0; i < empDetails.Length; i++)
                    {
                        MasterEmployeeDTO ss = new MasterEmployeeDTO();
                        ss.HRME_EmployeeFirstName = empDetails[i].HRME_EmployeeFirstName;
                        ss.HRME_EmployeeMiddleName = empDetails[i].HRME_EmployeeMiddleName;
                        ss.HRME_EmployeeLastName = empDetails[i].HRME_EmployeeLastName;
                        ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                        ss.HRME_DOB = empDetails[i].HRME_DOB;
                        ss.HRME_SalaryType = departmentlist.Where(t => t.HRMD_Id == empDetails[i].HRMD_Id).Select(t => t.HRMD_DepartmentName).FirstOrDefault();
                        ss.HRME_UINumber = designationlist.Where(t => t.HRMDES_Id == empDetails[i].HRMDES_Id).Select(t => t.HRMDES_DesignationName).FirstOrDefault();
                        employeeDetails.Add(ss);
                    }
                    dto.employeedetailList = employeeDetails.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex");
            }
            return dto;
        }

        public async Task<MasterEmployeeDTO> getNotification(MasterEmployeeDTO dto)
        {
            List<MasterEmployeeDTO> employeeDetailsINC = new List<MasterEmployeeDTO>();
            List<MasterEmployeeDTO> employeeDetailsPRO = new List<MasterEmployeeDTO>();
            double dbldays = 0;
            double aleryday = 0;
            string str = "";
            DateTime crntdate = DateTime.Today;
            var departmentlist = _HRMSContext.HR_Master_Department.Where(t => t.HRMD_ActiveFlag == true).ToArray();
            var designationlist = _HRMSContext.HR_Master_Designation.Where(t => t.HRMDES_ActiveFlag == true).ToArray();
            var alertdays = _HRMSContext.HR_Configuration.Where(t => t.MI_Id == 17).Select(t => t.HRC_AlertDay).FirstOrDefault();
            aleryday = Convert.ToDouble(alertdays);
            try
            {
                var empDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.HRME_DOJ != null).ToArray();
                for (int i = 0; i < empDetails.Length; i++)
                {
                    //DateTime newdate = empDetails[i].HRME_DOJ;
                    dbldays = (DateTime.Today - Convert.ToDateTime(empDetails[i].HRME_DOJ.ToString()).AddMonths(6)).TotalDays;
                    if (dbldays < aleryday && dbldays >= 0)
                    {
                        MasterEmployeeDTO ss = new MasterEmployeeDTO();
                        ss.HRME_EmployeeFirstName = (empDetails[i].HRME_EmployeeFirstName + " " + (empDetails[i].HRME_EmployeeMiddleName == null ? "" : empDetails[i].HRME_EmployeeMiddleName) + " " + (empDetails[i].HRME_EmployeeLastName == null ? "" : empDetails[i].HRME_EmployeeLastName));
                        ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                        ss.HRME_DOJ = empDetails[i].HRME_DOJ;
                        employeeDetailsINC.Add(ss);
                    }
                }
                //dto.employeedetailList = employeeDetails.ToArray();

                if (employeeDetailsINC.Count > 0)
                {
                    str += "<table width='100%' border= '1'><tr align='center' width='100%'>PROBATION ALERT</tr>" +
                        "<tr style = 'background-color:cornflowerblue'><td> EMPLOYEE NAME </td><td> EMPLOYEE CODE </td><td> DATE OF JOINING</td></tr> ";

                    for (int i = 0; i < employeeDetailsINC.Count; i++)
                    {
                        str += "<tr><td>" + employeeDetailsINC[i].HRME_EmployeeFirstName + "</td><td>" + employeeDetailsINC[i].HRME_EmployeeCode + "</td><td>" + Convert.ToDateTime(employeeDetailsINC[i].HRME_DOJ).ToString("dd-MM-yyyy") + "</td></tr>";
                    }
                    str += "</table>";
                    str += "<br /><br />";
                }

                empDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && t.HRME_DOJ != null).ToArray();
                //empDetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_ActiveFlag == true && t.HRME_LeftFlag == false && (DateTime.Today - Convert.ToDateTime(t.HRME_DOJ).AddMonths(6)).TotalDays == aleryday).ToArray();
                for (int i = 0; i < empDetails.Length; i++)
                {
                    dbldays = (DateTime.Today - Convert.ToDateTime(empDetails[i].HRME_DOJ).AddYears(1)).TotalDays;
                    if (dbldays < aleryday && dbldays >= 0)
                    {
                        MasterEmployeeDTO ss = new MasterEmployeeDTO();
                        ss.HRME_EmployeeFirstName = (empDetails[i].HRME_EmployeeFirstName + " " + (empDetails[i].HRME_EmployeeMiddleName == null ? "" : empDetails[i].HRME_EmployeeMiddleName) + " " + (empDetails[i].HRME_EmployeeLastName == null ? "" : empDetails[i].HRME_EmployeeLastName));
                        ss.HRME_EmployeeCode = empDetails[i].HRME_EmployeeCode;
                        ss.HRME_DOJ = empDetails[i].HRME_DOJ;
                        employeeDetailsPRO.Add(ss);
                    }
                    
                }
                //dto.employeedetailList = employeeDetails.ToArray();

                if (employeeDetailsPRO.Count > 0)
                {
                    str += "<table width='100%' border= '1'>" +
                        "<tr align='center' width='100%'>INCREMENT ALERT</tr>" +
                        "<tr style = 'background-color:cornflowerblue'><td> EMPLOYEE NAME </td><td> EMPLOYEE CODE </td><td> DATE OF JOINING</td></tr> ";

                    for (int i = 0; i < employeeDetailsPRO.Count; i++)
                    {
                        str += "<tr><td>" + employeeDetailsPRO[i].HRME_EmployeeFirstName + "</td><td>" + employeeDetailsPRO[i].HRME_EmployeeCode + "</td><td>" + Convert.ToDateTime(employeeDetailsPRO[i].HRME_DOJ).ToString("dd-MM-yyyy") + "</td></tr>";
                    }
                    str += "</table>";
                    str += "<br /><br />";
                }

                if(employeeDetailsINC.Count > 0 || employeeDetailsPRO.Count > 0)
                {
                    SendEmail("goutamkumar@vapstech.com", "Probation Increment Notification", str, dto.MI_Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex");
            }
            return dto;
        }

        public void SendEmail(string mailid, string subject, string body, long id)
        {
            //mailid = "goutamkumar@vapstech.com";
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _HRMSContext.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_Template_Name.Equals("Probation_Increment", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _HRMSContext.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _HRMSContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

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

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');
                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

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

                    using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _HRMSContext.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _HRMSContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _HRMSContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
            }
        }
    }
}
