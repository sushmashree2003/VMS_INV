using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class ConsolidatedSalaryReportDTO
    {
        public long userId { get; set; }
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long hrmeid { get; set; }
        public long HRME_Id { get; set; }
        public long HRMLY_Id { get; set; }
        public long IVRM_Month_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public string monthname { get; set; }
        public long? HRMLY_LeaveYearOrder { get; set; }
        public Array empnames { get; set; }
        public Array EmployeesalaryDetails { get; set; }
        public Array fillmonth { get; set; }
        public Array fillyear { get; set; }
        public string EmplYoeeName { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string HRME_LocStreet { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRME_Photo { get; set; }
        public DateTime? HRME_DOJ { get; set; }
        public long? HRME_MobileNo { get; set; }
        public long yearid { get; set; }
        public string month { get; set; }
        public string HRES_Month { get; set; }
        public long HRES_Year { get; set; }
        public DateTime Fromdate1 { get; set; }
        public DateTime Todate1 { get; set; }
        public Array employeeSalaryslipDetails { get; set; }
    }
}
