using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class PFReportsDTO
    {
        public long MI_Id { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }

        public Array employeedropdown { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array monthdropdown { get; set; }

        public long HRME_Id { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public Array pfreport { get; set; }
        public Array employeeDetails { get; set; }

        public Array institutionDetails { get; set; }

        public string finYearFromDate { get; set; }
        public string finYearToDate { get; set; }

        public string EmployeeCode { get; set; }
        public string departmentName { get; set; }
        public string designationName { get; set; }
        public string HRME_BankAccountNo { get; set; }
        public string HRME_LocStreet { get; set; }
        public string HRME_PerCity { get; set; }

        //
        public long HRESD_Id { get; set; }
        public long HRMED_Id { get; set; }
        public string HRMED_Name { get; set; }
        public decimal? HRESD_Amount { get; set; }

        public int? HRME_DOJ { get; set; }
        public decimal? AmountofWages { get; set; }
        public decimal? PFAmount { get; set; }

        public long HRES_Id { get; set; }
        public decimal? HRES_EPF { get; set; }
        public decimal? HRES_FPF { get; set; }

        public Array PayrollStandard { get; set; }

        public string FatherHusbandName { get; set; }

        public string HRME_PFAccNo { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }

        public DateTime? HRME_DOL { get; set; }
        public string HRME_LeavingReason { get; set; }

        public DateTime? HRME_PFDate { get; set; }
        public DateTime? HRME_DOB { get; set; }
        public string IVRMMG_GenderName { get; set; }
        public string TotalServicePeriod { get; set; }


        public decimal? HRES_Ac21 { get; set; }
        public decimal? HRES_Ac22 { get; set; }
        public decimal? HRES_Ac5 { get; set; }
        public decimal? HRES_Ac10 { get; set; }

        public string HRMED_EDTypeFlag { get; set; }

        public Array bankdetails { get; set; }

        public decimal? professionaltaxamount { get; set; }

        public string HRME_PFuAN { get; set; }

        public decimal? empGrossSal { get; set; }

        public decimal? netsalary { get; set; }
        public long?[] groupTypeIdList { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }
        
        public int abc { get; set; }
        public MasterEmployeeDTO currentemployeeDetails { get; set; }

        public int? H_DOB { get; set; }

        public class PFESIDetialsReportDTO
        {
            public long MI_Id { get; set; }
            public long roleId { get; set; }
            public Array employeePFESIDetialsReport { get; set; }
        }

        }
}
