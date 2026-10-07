using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Portals.Staff
{
    public class ClgStaffDashboardDTO
    {
        public int IVRMSTAUL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRME_Id { get; set; }
        public long UserId { get; set; }
        public long roleid { get; set; }



        public DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public string COEME_EventName { get; set; }
        public string COEME_EventDesc { get; set; }
        public DateTime? COEE_ReminderDate { get; set; }
        public long HRMEMNO_MobileNo { get; set; }
        public string HRMEM_EmailId { get; set; }
        public Array calenderlist { get; set; }
        public Array coereportlist { get; set; }
        
        public Array employeedetails { get; set; }
        public Array empmobileno { get; set; }
        public Array empemailid { get; set; }


    }
}

