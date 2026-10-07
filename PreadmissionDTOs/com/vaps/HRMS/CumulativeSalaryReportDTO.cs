using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class CumulativeSalaryReportDTO
    {
        public long HRES_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRESD_Id { get; set; }
        public Double? HRES_WorkingDays { get; set; }
        public DateTime? HRES_FromDate { get; set; }
        public DateTime? HRES_ToDate { get; set; }
        public string EmployeeName { get; set; }
        public decimal LOPDays { get; set; }
      
        public string HRME_EmployeeCode { get; set; }

    
        public HR_Employee_Salary_DetailsDTO[] earningresult { get; set; }

        public HR_Employee_Salary_DetailsDTO[] arrearresult { get; set; }

        public HR_Employee_Salary_DetailsDTO[] deductionresult { get; set; }

        public decimal? grossEarning { get; set; }

        public decimal? grossArrear { get; set; }
        public decimal? grossDeduction { get; set; }
        public decimal? netSalary { get; set; }

        public decimal? HRES_ESIEmplr { get; set; }

        public string HRME_PFAccNo { get; set; }

        public decimal? HRES_PFEmplr { get; set; }

        public string HRME_UINumber { get; set; }


        public string HRME_EmployeeFirstName { get; set; }

        public string HRME_EmployeeMiddleName { get; set; }

        public string HRME_EmployeeLastName { get; set; }

        public decimal? netCTC { get; set; }

        public int? HRME_EmployeeOrder { get; set; }

        public string HRMDES_Designationname { get; set; }

        public string HRMG_GradeName { get; set; }
        public int? HRMG_ORDER { get; set; }

        public decimal? HRESD_Amount { get; set; }
        public decimal? empGrossSal { get; set; }
        public long? HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }

        public bool HRES_ApproveFlg { get; set; }
        public string HRES_Year { get; set; }
        public string HRES_Month { get; set; }
        public string comm { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public decimal? grosspayhead { get; set; }

        public string columnnames { get; set; }
        public string totalworkingdays { get; set; }
        public string VMSCRT_BDailyReport { get; set; }
    }
}
