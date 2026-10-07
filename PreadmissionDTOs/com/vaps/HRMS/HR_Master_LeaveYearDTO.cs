using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_LeaveYearDTO:CommonParamDTO
    {
        public long HRMLY_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public DateTime HRMLY_FromDate { get; set; }
        public DateTime HRMLY_ToDate { get; set; }
        public bool HRMLY_ActiveFlag { get; set; }
        public Array leaveYearList { get; set; }
        public Array leaveYearListorderwise { get; set; }
        public string retrunMsg { get; set; }
        public long roleId { get; set; }
        public long? HRMLY_LeaveYearOrder { get; set; }
        public LeaveYearOrder[] LeaveYearOrder { get; set; }
        public long LogInUserId { get; set; }
    }

    public class LeaveYearOrder
    {
        public long HRMLY_Id { get; set; }
        public string HRMLY_LeaveYear { get; set; }
        public long? HRMLY_LeaveYearOrder { get; set; }
    }
}
