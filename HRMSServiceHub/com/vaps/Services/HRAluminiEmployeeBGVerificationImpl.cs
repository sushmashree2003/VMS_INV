using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using HRMSServiceHub.com.vaps.Interfaces;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Exit;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Services
{
    public class HRAluminiEmployeeBGVerificationImpl : HRAluminiEmployeeBGVerificationInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;

        public HRAluminiEmployeeBGVerificationImpl(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }


        public HRAluminiEmployeeBGVerificationDTO getBasicData(HRAluminiEmployeeBGVerificationDTO dto)
        {

            try
            {
                //employee
                dto.employees = (
                                 from mm in _HRMSContext.MasterEmployee
                                 where mm.HRME_ActiveFlag == true
                                 orderby mm.HRME_EmployeeOrder
                                 select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                dto.griddata = (from a in _HRMSContext.HRAluminiEmployeeBGVerificationDMO
                                from b in _HRMSContext.MasterEmployee
                                where a.HRME_Id == b.HRME_Id
                                select new HRAluminiEmployeeBGVerificationDTO
                                {
                                    HRMEVER_Id = a.HRMEVER_Id,
                                    HRMEVER_EmailId = a.HRMEVER_EmailId,
                                    HRME_Id = a.HRME_Id,
                                    HRMEVER_RequestedCompanyName = a.HRMEVER_RequestedCompanyName,                           
                                    HRMEVER_ContactNo = a.HRMEVER_ContactNo,
                                    HRMEVER_ActiveFlg = a.HRMEVER_ActiveFlg,
                                    HRMEMNO_RequestedBy=a.HRMEMNO_RequestedBy,
                                    HRMEVER_VerificationCode=a.HRMEVER_VerificationCode,
                                    HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                    HRMEVER_GeneratedDate=a.HRMEVER_GeneratedDate,
                                    HRMEVER_ExpiryDate = a.HRMEVER_ExpiryDate,
                                }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        // Mail 
        public HRAluminiEmployeeBGVerificationDTO sendResignationMail(HRAluminiEmployeeBGVerificationDTO dto)
        {
            try
            {
                //var employees = (
                //                 from mm in _HRMSContext.MasterEmployee
                //                 where mm.HRME_ActiveFlag == true
                //                 orderby mm.HRME_EmployeeOrder
                //                 select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                var department = (from a in _HRMSContext.HRAluminiEmployeeBGVerificationDMO
                                  from b in _HRMSContext.MasterEmployee
                                  where (a.HRME_Id == b.HRME_Id && a.HRME_Id == dto.HRME_Id)
                                  select b).Select(t => t.HRME_EmployeeFirstName).FirstOrDefault();

                string mailid = dto.HRMEVER_EmailId;
                    var institutionName = _Context.Institution.Where(i => i.MI_Id == dto.MI_Id).ToList();

                string Mailmsg = "Hello " + department + ", Your verification code for " + DateTime.Now.ToString("dd/MM/yyyy") + "." +
                     "\n\nVerification Code: " + dto.HRMEVER_VerificationCode +
                     "\n\nWebsite Link: " + "https://singularitydevs.com/" + "." +
                     "\n\nPlease let me know if there is anything further I can do to assist in this process.  \n\nThank You";

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                    alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(dto.MI_Id)).ToList();

                    if (alldetails.Count > 0)
                    {
                        string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                        string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                        string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                        string mailcc = alldetails[0].IVRM_mailcc;
                        Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                        string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                        string Subject = "Devs Team";
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(mailid);

                        StringBuilder sb = new StringBuilder(dto.Template);
                        StringReader sr = new StringReader(sb.ToString());
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {                           
                            message.HtmlContent = Mailmsg;
                            var client = new SendGridClient(sengridkey);
                            client.SendEmailAsync(message).Wait();
                        }

                        using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _Context.SMSEmailSetting.Where(e => e.ISES_Template_Name.Equals("[Verification_Code]", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_Email_Outgoing_1";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                            {
                                Value = mailid
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                            {
                                Value = Mailmsg
                            });
                            cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                            {
                                Value = "Devs Team"
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = dto.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
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

                    dto.retrunMsg = "Success";               
            }
            catch (Exception ex)
            {
                dto.retrunMsg = "False";
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

      //mail end//

        public HRAluminiEmployeeBGVerificationDTO deactivate(HRAluminiEmployeeBGVerificationDTO data)
        {
            try
            {
                var result = _HRMSContext.HRAluminiEmployeeBGVerificationDMO.Single(t => t.HRMEVER_Id == data.HRMEVER_Id && t.HRME_Id == data.HRME_Id);

                if (result.HRMEVER_ActiveFlg == true)
                {
                    result.HRMEVER_ActiveFlg = false;
                }
                else if (result.HRMEVER_ActiveFlg == false)
                {
                    result.HRMEVER_ActiveFlg = true;
                }
                result.HRMEVER_UpdatedDate = DateTime.Now;
                _HRMSContext.Update(result);

                int rowAffected = _HRMSContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.retrunMsg = "true";
                }
                else
                {
                    data.retrunMsg = "false";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public HRAluminiEmployeeBGVerificationDTO Editdata(HRAluminiEmployeeBGVerificationDTO data)
        {
            try
            {

                data.getdetails = (from a in _HRMSContext.HRAluminiEmployeeBGVerificationDMO
                                                  from b in _HRMSContext.MasterEmployee
                                                  where a.HRME_Id == b.HRME_Id && a.HRMEVER_Id == data.HRMEVER_Id
                                                  select new HRAluminiEmployeeBGVerificationDTO
                                                  {
                                                      HRMEVER_Id = a.HRMEVER_Id,
                                                      HRMEVER_EmailId = a.HRMEVER_EmailId,
                                                      HRME_Id = a.HRME_Id,
                                                      HRMEVER_RequestedCompanyName = a.HRMEVER_RequestedCompanyName,
                                                      HRMEVER_ContactNo = a.HRMEVER_ContactNo,
                                                      HRMEVER_ActiveFlg = a.HRMEVER_ActiveFlg,
                                                      HRMEMNO_RequestedBy = a.HRMEMNO_RequestedBy,
                                                      HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                                                      HRMEVER_GeneratedDate = a.HRMEVER_GeneratedDate,
                                                      HRMEVER_ExpiryDate = a.HRMEVER_ExpiryDate,
                                                  }).Distinct().ToArray();

            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

        public HRAluminiEmployeeBGVerificationDTO savedata(HRAluminiEmployeeBGVerificationDTO dto)
        {
            try
            {
                var employeedetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == dto.HRME_Id).ToList();
                var varificationcode = employeedetails[0].HRME_EmployeeFirstName + employeedetails[0].HRME_EmployeeCode;
                if (dto.HRMEVER_Id > 0)
                {
                    var result = _HRMSContext.HRAluminiEmployeeBGVerificationDMO.Single(t => t.HRMEVER_Id == dto.HRMEVER_Id);
                    result.HRMEMNO_RequestedBy = dto.HRMEMNO_RequestedBy;
                    result.HRMEVER_EmailId = dto.HRMEVER_EmailId;
                    result.HRMEVER_ContactNo = dto.HRMEVER_ContactNo;
                    result.HRMEVER_RequestedCompanyName = dto.HRMEVER_RequestedCompanyName;
                    result.HRMEVER_VerificationCode = varificationcode;
                    result.HRMEVER_GeneratedDate = dto.HRMEVER_GeneratedDate;
                    result.HRMEVER_ExpiryDate = dto.HRMEVER_ExpiryDate;
                    _HRMSContext.Update(result);
                    _HRMSContext.SaveChanges();
                    dto.returnvales = "Update";
                }
                else
                {
                    HRAluminiEmployeeBGVerificationDMO hm = new HRAluminiEmployeeBGVerificationDMO();
                    hm.HRMEVER_Id = dto.HRMEVER_Id;
                    hm.HRMEMNO_RequestedBy = dto.HRMEMNO_RequestedBy;
                    hm.HRMEVER_EmailId = dto.HRMEVER_EmailId;
                    hm.HRMEVER_ContactNo = dto.HRMEVER_ContactNo;
                    hm.HRMEVER_RequestedCompanyName = dto.HRMEVER_RequestedCompanyName;
                    hm.HRME_Id = dto.HRME_Id;
                    hm.HRMEVER_GeneratedDate = dto.HRMEVER_GeneratedDate;
                    hm.HRMEVER_ExpiryDate = dto.HRMEVER_ExpiryDate;
                    hm.HRMEVER_VerificationCode = varificationcode;
                    hm.HRMEVER_ActiveFlg = true;
                    hm.HRMEVER_CreatedBy = dto.userId;
                    hm.HRMEVER_UpdatedBy = dto.userId;
                    hm.HRMEVER_CreatedDate = DateTime.Now;
                    hm.HRMEVER_UpdatedDate = DateTime.Now;
                    _HRMSContext.Add(hm);
                    _HRMSContext.SaveChanges();
                    dto.returnvales = "Add";
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public ISM_Resignation_DTO FANDFSettlement(ISM_Resignation_DTO dto)
        {
            try
            {

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Leave_Encash";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt) //HRME_Id=1698
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.LeaveEnchashmentData = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GratuityCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.gratuitydetails = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FANDF_TADA_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.tadadetails = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_EMP_Loan_PendingDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.loandetails = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exit_Damage_Amount";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                    {
                        Value = dto.HRME_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var retObject11 = new List<dynamic>();
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of 
                                    );
                                }
                                retObject11.Add((ExpandoObject)dataRow1);
                            }
                        }
                        dto.demagedetails = retObject11.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

              //  dto.bankdetails = _HRMSContext.HR_Master_BankDeatils.Where(t => t.MI_Id == dto.MI_Id && t.HRMBD_ActiveFlag == true).ToArray();
                //dto.ffdetailsarray = _HRMSContext.ISM_Resignation_DMO.Where(t => t.ISMRESG_Id == dto.ISMRESG_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }


        public ISM_Certificate_RequestDTO nocdetails(ISM_Certificate_RequestDTO data)
        {
            try
            {

                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_NocApprovedDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.aprovedlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (System.Data.Common.DbCommand cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_NOCApproved_Checkilists";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.checklist = retObject.ToArray();
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
            return data;


        }
    }
}
