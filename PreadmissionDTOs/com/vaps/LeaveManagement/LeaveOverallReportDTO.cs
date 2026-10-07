using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class LeaveOverallReportDTO
    {

        public long MI_Id { get; set; }
        public string IVRM_Month_Name { get; set; }
        public long HRML_Id { get; set; }
        public decimal HRELT_TotDays { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRELAP_ApplicationStatus { get; set; }
        public Array leave_name { get; set; }
        public Array institutionlist { get; set; }
        public string HRML_LeaveName { get; set; }
        public Array approval_name { get; set; }
        public Array getmonthReport { get; set; }

        public DateTime? HRELAP_FromDate { get; set; }
        public DateTime? HRELAP_ToDate { get; set; }
        public long Institute_Id { get; set; }
        public string Rdotype { get; set; }
        public string Radioflag { get; set; }

        public Array get_emp { get; set; }
        public DateTime? FROM_DATE { get; set; }

        public DateTime? TO_DATE { get; set; }
        public Array getReport { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        public string HRMD_DepartmentName { get; set; }
        public long HRME_Id { get; set; }
        public string HRML_LeaveCode { get; set; }
        public string selectedLeave { get; set; }

        public Array getmonth { get; set; }
        public Array getyear { get; set; }
        public long year { get; set; }
        public int month { get; set; }
        public long HRMLY_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public long? HRMLY_LeaveYearOrder { get; set; }
        public string monthname { get; set; }
        public long IVRM_Month_Id { get; set; }
        public string MI_Name{ get; set; }


    }
}
