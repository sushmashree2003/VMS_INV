using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class EmployeeLogReportDTO
    {


        public long MI_Id { get; set; }
        public Array filltypes { get; set; }
        public Array filldepartment { get; set; }
        public Array filldesignation { get; set; }
        public Array fillemployee { get; set; }
        public Array filldata { get; set; }      
        public string multipletype { get; set; }
        public string multipledep { get; set; }
        public string multipledes { get; set; }
        public string multiplehrmeid { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string selectdate { get; set; }
        public string selectmonth { get; set; }
        public string selectyear { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string punchtype { get; set; }
        public Array fillmonth { get; set; }
        public int monthid { get; set; }
        public string monthname { get; set; }
        public Array fillyear { get; set; }
        public int yearid { get; set; }
        public long IVRM_Month_Id { get; set; }
        public bool HRME_LeftFlag { get; set; }

        public Array employeename { get; set; }
        public long userId { get; set; }
        public long hrmeid { get; set; }
        public long HRMLY_Id { get; set; }       
        public string EmplYoeeName { get; set; }
        public Array logindetails { get; set; }
        public Array AllFOHRAttendenceReportMD { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }

        public string Message { get; set; }
        public string afu_filename { get; set; }
        public string HRRemarks { get; set; }
        public string afu_filepath { get; set; }
        public DateTime? afu_date { get; set; }
        public Array AllFOHRAttendence { get; set; }
    }
}
