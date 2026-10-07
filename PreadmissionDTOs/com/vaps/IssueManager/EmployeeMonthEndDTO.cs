using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class EmployeeMonthEndDTO
    {
        public Array employeelist { get; set; }
        public Array monthlist { get; set; }
        public Array yearlist { get; set; }
        public Array employeedetails { get; set; }
        public Array companydetails { get; set; }
        public long HRME_Id { get; set; }
        public long VMSEWMER_Month { get; set; }
        public long VMSEWMER_Year { get; set; }
        public string HRME_EmployeeFirstName { get; set; }

        
        public long IVRM_Month_Id { get; set; }
        public long HRMLY_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string monthname { get; set; }

        public Array rndcount { get; set; }
        public Array allheadvalues { get; set; }
        public long MI_Id { get; set; }
        public long? HRMDC_ID { get; set; }
        public long plannedtaskcount { get; set; }
        public long VMSMMEDPT_Id { get; set; }

        public long drgeneratedcount { get; set; }
        public decimal? drhoursplanned { get; set; }
        public decimal? drhoursworked { get; set; }
        public decimal? drhoursapproved { get; set; }
        public decimal? drhoursarejected { get; set; }

        public long workingdayscount { get; set; }
        public long presentdayscount { get; set; }
        public long leavescount { get; set; }
        public long appliedleavescount { get; set; }
        public long approvedleavescount { get; set; }
        public long rejectedleavescount { get; set; }
        public long UserId { get; set; }
        public string message { get; set; }
        public string VMSMMEDPT_Description { get; set; }
        public bool VMSMMEDPT_ActiveFlag { get; set; }
        public bool? VMSMMEDPT_ManualFlag { get; set; }
        public string values { get; set; }
        public string remark { get; set; }
        public string VMSEWMER_Remarks { get; set; }
        public string VMSEWMER_Value { get; set; }
        public bool valueexist { get; set; }

        public Array Dailyreptnotsent { get; set; }
        public Array staffleaveDetails { get; set; }
        public Array CriticalCount { get; set; }
        public Array MajortaskCount { get; set; }
        public Array MinortaskCount { get; set; }
        public Array CompletedtaskCount { get; set; }
        public Array IssueOpenCount { get; set; }
        public Array IssuecompleteCount { get; set; }
        public Array EnhancementOpenCount { get; set; }
        public Array EnhancementcompltCount { get; set; }
        public Array OverallDeveloperCounts { get; set; }
        public List<DEVDTO> MonthlyDeviation { get; set; }
        public EmployeeMonthEndDTO[] TempDTO { get; set; }
        public string HRMD_DepartmentName { get; set; }

        //added by roopa
        public Array Names { get; set; }
        public string HRME_EmployeeFirstNametwo { get; set; }

      //  public Array Reporting_mgr { get; set; }
        public string rpt_name { get; set; }

    }
}
