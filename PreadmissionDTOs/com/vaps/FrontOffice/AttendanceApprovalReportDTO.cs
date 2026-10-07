using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class AttendanceApprovalReportDTO
    {
        public string HREAA_Remarks { get; set; }

        public long HREAA_SanctionLevel { get; set; }

        public string HREAA_ApprovalFlg { get; set; }

        //public bool Is_Active { get; set; }

        public Array getmonth { get; set; }

        public long MI_Id { get; set; }

        public string IVRM_Month_Name { get; set; }
        public Array getyear { get; set; }

        public Array institutionlist { get; set; }

        public long HRMLY_LeaveYear { get; set; }
        public Array getreport { get; set; }
        public string HRME_EmployeeCode {get;set;}
        public long HRME_ID { get; set; }

        public institutelist[] selected_Inst { get; set; }
    }

   public class institutelist
    {
        public long MI_Id { get; set; }
        public string MI_Name { get; set; }
    }

}
