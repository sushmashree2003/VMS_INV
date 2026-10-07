using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Employee_Salary_PaymentDetailsDTO
    {
        public long HRESPDET_Id { get; set; }
        public long HRES_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRESPDET_Year { get; set; }
        public string HRESPDET_Month { get; set; }
        public decimal HRESPDET_Percentage { get; set; }
        public decimal HRESPDET_Amount { get; set; }
        public DateTime? HRESPDET_PaymentDate { get; set; }
        public string HRESPDET_ModeOfPayment { get; set; }
        public string HRESPDET_Remarks { get; set; }
        public bool HRESPDET_ActiveFlg { get; set; }
        public DateTime? HRESPDET_CreatedDate { get; set; }
        public long HRESPDET_CreatedBy { get; set; }
        public DateTime? HRESPDET_UpdatedDate { get; set; }
        public long HRESPDET_UpdatedBy { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public string IVRM_Month_Name { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMED_EarnDedFlag { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public decimal? HRESD_Amount { get; set; }
        public decimal revisedSalary { get; set; }
        public string empIds { get; set; }
        public long UserId { get; set; }

        public bool selected { get; set; }

        public Array leaveyeardropdown { get; set; }
        public Array monthdropdown { get; set; }
        public Array employeeDetails { get; set; }
        public List<long> empid { get; set; }
        public InstitutionDTO institutionDetails { get; set; }
        public Array main_list { get; set; }
        public HR_Employee_Salary_PaymentDetailsDTO[] ApprovalTempDTO { get; set; }
    }
}
