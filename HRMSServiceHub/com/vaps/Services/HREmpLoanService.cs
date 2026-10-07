using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
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
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class HREmpLoanService : Interfaces.HREmpLoanInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public HREmpLoanService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }

        public HR_Emp_LoanDTO getBasicData(HR_Emp_LoanDTO dto)
        {

            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Emp_LoanDTO SaveUpdate(HR_Emp_LoanDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO conobj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);

                if (conobj.HRC_LoanApprovalFlg == true)
                {
                    dto.HREL_LoanStatus = "Applied";
                }
                else
                {
                    dto.HREL_LoanStatus = "Sanctioned";
                    dto.HREL_SanctionedAmount = dto.HREL_LoanAmount;
                    dto.HREL_TotalPending = dto.HREL_LoanAmount;
                }


                HR_Emp_Loan dmoObj = Mapper.Map<HR_Emp_Loan>(dto);


                if (dmoObj.HREL_Id > 0)
                {
                    //Array previous_Noresultremove = _HRMSContext.HR_Emp_Loan_Documents.Where(t => !filesid.Contains(t.TENANTAGRFL_Id)
                    //      && t.TENANTAGRFL_Id == data.TENANTAGRFL_Id).ToArray();

                    //foreach (TenantAgreementFiles ph1 in previous_Noresultremove)
                    //{
                    //    _context.Remove(ph1);
                    //}


                    var result = _HRMSContext.HR_Emp_Loan.Single(t => t.HREL_Id == dmoObj.HREL_Id);
                    dto.UpdatedDate = DateTime.Now;
                    result.HREL_UpdatedBy = dto.LogInUserId;
                    Mapper.Map(dto, result);
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
                else
                {

                    var duplicateaccno = _HRMSContext.HR_Emp_Loan.Where(t => t.MI_Id == dto.MI_Id && t.HRME_Id.Equals(dto.HRME_Id) && t.HREL_TotalPending > 0).Count();
                    if (duplicateaccno == 0)
                    {

                        var Master_Numbering = _Context.Master_Numbering.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.IMN_Flag == "Loan").ToList();
                        if (Master_Numbering.Count() > 0)
                        {
                            dto.transnumconfigsettings = Mapper.Map<Master_NumberingDTO>(Master_Numbering.FirstOrDefault());
                        }
                        else
                        {
                            dto.retrunMsg = "EmployeeLoanNumbernotGenerated";
                            return dto;
                        }


                        if (dto.transnumconfigsettings != null)
                        {
                            if (dto.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                            {

                                dto.transnumconfigsettings.MI_Id = dto.MI_Id;
                                dto.transnumconfigsettings.ASMAY_Id = dto.ASMAY_Id;
                                dmoObj.HREL_EmpLoanId = GenerateNumber(dto.transnumconfigsettings);

                            }
                            else
                            {
                                dmoObj.HREL_EmpLoanId = dto.HREL_EmpLoanId;
                            }

                        }
                        else
                        {
                            dto.retrunMsg = "EmployeeLoanNumbernotGenerated";
                            return dto;
                        }


                        dmoObj.HREL_ActiveFlag = true;
                        //dmoObj.HREL_AppliedDate = DateTime.Now;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        dmoObj.HREL_UpdatedBy = dto.LogInUserId;
                        dmoObj.HREL_CreatedBy = dto.LogInUserId;
                        _HRMSContext.Add(dmoObj);

                        if (dto.teacherdocuupload != null && dto.teacherdocuupload.Length > 0)
                        {
                            foreach (var c in dto.teacherdocuupload)
                            {
                                if (c.HRELD_Filepath != null && c.HRELD_Filepath != "")
                                {
                                    HR_Emp_Loan_Documents resultfile = new HR_Emp_Loan_Documents();
                                    resultfile.HREL_ID = dmoObj.HREL_Id;
                                    resultfile.HRMDOC_Id = c.HRMDOC_Id;
                                    resultfile.HRELD_Filepath = c.HRELD_Filepath;
                                    resultfile.HRELD_Filetype = c.HRELD_Filetype;
                                    resultfile.HRELD_ActiveFlag = true;
                                    resultfile.Createdby = dto.LogInUserId;
                                    resultfile.Createddate = DateTime.Now;
                                    resultfile.Updateddate = DateTime.Now;
                                    resultfile.Updatedby = dto.LogInUserId;



                                    _HRMSContext.Add(resultfile);
                                }
                            }
                        }
                        var hrpaid = _HRMSContext.HR_PROCESSDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRPA_TypeFlag == "Loan").Select(t => t.HRPA_Id).FirstOrDefault();

                        var finalflag = _HRMSContext.HR_Process_Auth_OrderNoDMO.Where(t => t.HRPA_Id == hrpaid && t.HRPAON_SanctionLevelNo == 1).Select(t => t.IVRMUL_Id).FirstOrDefault();

                        var emailid = _HRMSContext.Emp_Email_Id.Where(t => t.HRME_Id == dto.HRME_Id).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                        var toemailid1 = _HRMSContext.Staff_User_Login.Where(t => t.Id == finalflag).Select(t => t.Emp_Code).FirstOrDefault();

                        var toemail = _HRMSContext.Emp_Email_Id.Where(t => t.HRME_Id == toemailid1).Select(t => t.HRMEM_EmailId).FirstOrDefault();

                        var flag = _HRMSContext.SaveChanges();
                        if (flag > 1)
                        {
                            dto.retrunMsg = "Add";
                            sendmailcertificate(dto.MI_Id, "LOANREQUEST", dto.HRME_Id, toemail, emailid);

                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Duplicate";
                        return dto;
                    }

                }


                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Emp_LoanDTO editData(int id)
        {

            HR_Emp_LoanDTO dto = new HR_Emp_LoanDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Emp_Loan> lorg = new List<HR_Emp_Loan>();
                lorg = _HRMSContext.HR_Emp_Loan.AsNoTracking().Where(t => t.HREL_Id.Equals(id)).ToList();
                dto.emploanList = lorg.ToArray();

                dto = Mapper.Map<HR_Emp_LoanDTO>(lorg.FirstOrDefault());
                dto.emploanList = lorg.ToArray();

                var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning")
                                  select new HR_Employee_Salary_DetailsDTO
                                  {

                                      HRMED_Id = emp.HRMED_Id,
                                      HRMED_Name = mas.HRMED_Name,
                                      HRESD_Amount = emp.HREED_Amount,
                                      HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                                  }).ToList();

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Emp_LoanDTO getDetailsByEmployee(HR_Emp_LoanDTO dto)
        {
            dto.empGrossSal = 0;
            // dto.totalAppliedAmount = 0;


            try
            {
                var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning")
                                  select new HR_Employee_Salary_DetailsDTO
                                  {

                                      HRMED_Id = emp.HRMED_Id,
                                      HRMED_Name = mas.HRMED_Name,
                                      HRESD_Amount = emp.HREED_Amount,
                                      HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                                  }).ToList();

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
                //  GetTotalAppliedAmount(dto);




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return dto;
        }


        public HR_Emp_LoanDTO deactivate(HR_Emp_LoanDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HREL_Id > 0)
                {
                    var result = _HRMSContext.HR_Emp_Loan.Single(t => t.HREL_Id == dto.HREL_Id);

                    if (result.HREL_ActiveFlag == true)
                    {
                        result.HREL_ActiveFlag = false;
                    }
                    else if (result.HREL_ActiveFlag == false)
                    {
                        result.HREL_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    result.HREL_UpdatedBy = dto.LogInUserId;
                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HREL_ActiveFlag == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Emp_LoanDTO loanchange(HR_Emp_LoanDTO dto)
        {
            dto.retrunMsg = "";
            try
            {

                var loancode = _HRMSContext.HRMasterLoan.Where(t => t.HRMLN_Id == dto.HRMLN_Id && t.MI_Id == dto.MI_Id).ToList();

                dto.docdetails = _HRMSContext.HR_Master_Documents.Where(t => t.MI_Id == dto.MI_Id && t.HRMDOC_Doctype == loancode[0].HRMLN_Loancode).ToArray();

                dto.HREL_LoanInterest = _HRMSContext.HRMasterLoan.Where(t => t.HRMLN_Id == dto.HRMLN_Id && t.MI_Id == dto.MI_Id).Select(t => t.HRML_InterestAmount).SingleOrDefault();

                dto.docdetails = (from a in _HRMSContext.HR_Master_Documents

                                  where (a.MI_Id == dto.MI_Id && a.HRMDOC_Doctype == loancode[0].HRMLN_Loancode && a.HRMDOC_FLAG == true)
                                  select new HR_Emp_LoanDTO
                                  {
                                      HRMDOC_DocumentName = a.HRMDOC_DocumentName,
                                      HRMDOC_Description = a.HRMDOC_Description,
                                      HRMDOC_Id = a.HRMDOC_Id,
                                      HRMDOC_FLAG = a.HRMDOC_FLAG,
                                      HRELD_Filepath = "",
                                      HRELD_Filetype = ""




                                  }).ToArray();
                using (var cmd = _HRMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Loanprocess_Check";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID", SqlDbType.BigInt) { Value = dto.HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@HRMLN_Id", SqlDbType.BigInt) { Value = dto.HRMLN_Id });





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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.loanchange = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }


        public HR_Emp_LoanDTO GetAllDropdownAndDatatableDetails(HR_Emp_LoanDTO dto)
        {
            var LogInUserId = _HRMSContext.Staff_User_Login.Where(t => t.Id == dto.LogInUserId).ToList();

            List<HR_Emp_Loan> datalist = new List<HR_Emp_Loan>();

            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HRMasterLoan> masteloan = new List<HRMasterLoan>();
            try
            {


                var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true && t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

                HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                dto.configurationDetails = dmoObj;


                var employees = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                 from mm in _HRMSContext.MasterEmployee
                                 from med in _HRMSContext.HR_Master_EarningsDeductions
                                 where mm.MI_Id.Equals(dto.MI_Id)
                                 && emp.HRMED_Id == med.HRMED_Id
                                 && med.HRMED_EDTypeFlag == "Loan" && emp.HREED_ActiveFlag == true
                                 && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id && mm.HRME_Id == LogInUserId[0].Emp_Code
                                 orderby mm.HRME_EmployeeOrder
                                 select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();



                dto.employeedropdown = employees.ToArray();
                if (employees.Count() > 0)
                {

                    var empIds = employees.Select(t => t.HRME_Id).ToArray();

                    datalist = _HRMSContext.HR_Emp_Loan.Where(t => t.MI_Id.Equals(dto.MI_Id) && empIds.Contains(t.HRME_Id)).ToList();
                    dto.emploanList = datalist.ToArray();
                }



                masteloan = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLN_ActiveFlag == true).ToList();
                dto.masterloandropdown = masteloan.ToArray();





            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


        public string GenerateNumber(Master_NumberingDTO en)
        {
            string GeneratedNumber = "";

            if (en != null)
            {
                if (en.IMN_AutoManualFlag == "Manual")
                {
                    GeneratedNumber = "";
                }
                else if (en.IMN_AutoManualFlag == "Auto")
                {
                    var acyr = _Context.AcademicYear.Where(t => t.ASMAY_Id.Equals(en.ASMAY_Id)).FirstOrDefault();
                    string AcadYear = acyr.ASMAY_Year;
                    // string[] a = AcadYear.Split('-');

                    // AcadYear = a.ElementAt(0);

                    if (en.IMN_PrefixAcadYearCode == true)
                    {
                        GeneratedNumber = AcadYear;
                    }

                    else
                    if (en.IMN_PrefixParticular != "" && en.IMN_PrefixParticular != null)
                    {
                        GeneratedNumber = en.IMN_PrefixParticular;
                    }

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                                GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }
                            else
                            {
                                en.IMN_StartingNo = "0";
                                GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }
                        }
                        else
                        {
                            if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                            {
                                GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString();
                            }
                        }

                    }
                    else
                    {
                        if (en.IMN_StartingNo != "" && en.IMN_StartingNo != null)
                        {
                            GeneratedNumber = GeneratedNumber + "/" + en.IMN_StartingNo.ToString();
                        }
                    }

                    if (en.IMN_SuffixAcadYearCode == true)
                    {
                        GeneratedNumber = GeneratedNumber + "/" + AcadYear;
                    }
                    else if (en.IMN_SuffixParticular != "" && en.IMN_SuffixParticular != null)
                    {
                        GeneratedNumber = GeneratedNumber + "/" + en.IMN_SuffixParticular;
                    }

                    GeneratedNumber = TransactionNumberingType(GeneratedNumber, en);
                }
            }

            return GeneratedNumber;

        }

        public string TransactionNumberingType(string GeneratedNumber, Master_NumberingDTO en)
        {
            if (en.IMN_Flag == "Loan")
            {
                GeneratedNumber = checkDublicateandIncreamentForLoan(GeneratedNumber, en);
            }
            return GeneratedNumber;
        }


        //Loan
        public string checkDublicateandIncreamentForLoan(string GeneratedNumber, Master_NumberingDTO en)
        {
            int count = 0;
            if (en.IMN_RestartNumFlag == "Never")
            {
                count = _HRMSContext.HR_Emp_Loan.Where(imp => imp.MI_Id == en.MI_Id && imp.HREL_EmpLoanId.Equals(GeneratedNumber)).Count();
            }
            else if (en.IMN_RestartNumFlag == "Yearly")
            {
                count = _HRMSContext.HR_Emp_Loan.Where(imp => imp.MI_Id == en.MI_Id && imp.HREL_EmpLoanId.Equals(GeneratedNumber)).Count();
            }
            if (count > 0)
            {
                string[] lastRecordArray = GeneratedNumber.Split('/');
                if (lastRecordArray != null)
                {
                    string lastfield = "";
                    string firstfield = lastRecordArray.ElementAt(0);
                    int staringNumber = Convert.ToInt32(lastRecordArray.ElementAt(1));
                    if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                    {
                        lastfield = "/" + lastRecordArray.ElementAt(2);
                    }

                    int staringNumberInc = staringNumber + 1;

                    if (en.IMN_WidthNumeric != "" && en.IMN_WidthNumeric != null)
                    {

                        if (en.IMN_ZeroPrefixFlag == "Yes")
                        {
                            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0') + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString().PadLeft(Convert.ToInt32(en.IMN_WidthNumeric) - 0, '0');
                            }

                        }
                        else
                        {
                            if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                            }
                            else
                            {
                                GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (en.IMN_SuffixAcadYearCode != false && en.IMN_SuffixCalYearCode != false && en.IMN_SuffixParticular != null)
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString() + lastfield;
                        }
                        else
                        {
                            GeneratedNumber = firstfield + "/" + staringNumberInc.ToString();
                        }
                    }
                }

                GeneratedNumber = checkDublicateandIncreamentForLoan(GeneratedNumber, en);
            }

            return GeneratedNumber;
        }

        public string sendmailcertificate(long MI_Id, string Template, long? HRME_ID, string email, string ccemail)        {            try            {                Dictionary<string, string> val = new Dictionary<string, string>();                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name == Template                && e.ISES_MailActiveFlag == true).ToList();                if (template.Count == 0)                {                    return "Email Template not Mapped to the selected Institution";                }                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;                string Resultsms = Mailcontent;                string result = Mailmsg;                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "LOANAPPROVALREPLACE";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",                  SqlDbType.VarChar)                    {                        Value = HRME_ID                    });                    cmd.Parameters.Add(new SqlParameter("@Type",                  SqlDbType.VarChar)                    {                        Value = Template                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                        dataReader.GetName(iFiled),                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                    var datatype = dataReader.GetFieldType(iFiled);                                    if (datatype.Name == "DateTime")                                    {                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else if (datatype.Name == "Decimal")                                    {                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);                                    }                                    else                                    {                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());                                    }                                }                            }                        }                    }                    catch (Exception ex)                    {                        return ex.Message;                    }                }                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailmsg = result;                        }                    }                }                Mailmsg = result;                for (int j = 0; j < ParamaetersName.Count; j++)                {                    for (int p = 0; p < val.Count; p++)                    {                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))                        {


                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);                            Mailcontent = Resultsms;                        }                    }                }                Mailcontent = Resultsms;                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();                string Attechement = "";                if (alldetails.Count > 0)                {                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);                    string Subject = template[0].ISES_MailSubject.ToString();                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();                    smstpdetails = _HRMSContext.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();                    string mailcc = "";                    string mailbcc = "";                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")                    {                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');                        mailcc = ccmail[0].ToString();                        if (ccmail.Length > 1)                        {                            if (ccmail[1] != null || ccmail[1] != "")                            {                                mailbcc = ccmail[1].ToString();                            }                        }                    }                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")                    {                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();                    }                    var message = new SendGridMessage();                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);                    message.Subject = Subject;                    message.AddTo(email);


                    //mailto:message.addto("sreepriyagangadharan15@gmail.com");

                    if (Attechement.Equals("1"))                    {                        var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();                        if (img.Count > 0)                        {                            for (int i = 0; i < img.Count; i++)                            {                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;                                Stream stream = response.GetResponseStream();                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);                            }                        }                    }
                    //***************** EMAIL CC DETAILS **************//

                    if (ccemail != null && ccemail != "")                    {                        string[] ccmaildetails = ccemail.Split(',');                        foreach (var c in ccmaildetails)                        {                            message.AddCc(c);                        }                    }






                    if (mailcc != null && mailcc != "")                    {
                        //message.AddCc(mailcc);
                    }                    if (mailbcc != null && mailbcc != "")                    {                        message.AddBcc(mailbcc);                    }                    message.HtmlContent = Mailmsg;                    var client = new SendGridClient(sengridkey);                    try                    {                        client.SendEmailAsync(message).Wait();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())                    {                        var template1010 = _HRMSContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();                        cmd.CommandText = "IVRM_Email_Outgoing";                        cmd.CommandType = CommandType.StoredProcedure;                        cmd.Parameters.Add(new SqlParameter("@EmailId",                            SqlDbType.NVarChar)                        {                            Value = email                        });                        cmd.Parameters.Add(new SqlParameter("@Message",                           SqlDbType.NVarChar)                        {                            Value = Mailcontent                        });                        cmd.Parameters.Add(new SqlParameter("@module",                        SqlDbType.VarChar)                        {                            Value = modulename[0]                        });                        cmd.Parameters.Add(new SqlParameter("@MI_Id",                       SqlDbType.BigInt)                        {                            Value = MI_Id                        });                        if (cmd.Connection.State != ConnectionState.Open)                            cmd.Connection.Open();                        try                        {                            using (var dataReader = cmd.ExecuteReader())                            {                            }                        }                        catch (Exception ex)                        {                            Console.WriteLine(ex.Message);                        }                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return "success";        }



    }
}
