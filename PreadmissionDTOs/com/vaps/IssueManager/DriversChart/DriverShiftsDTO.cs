using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.DriversChart
{
   public class DriverShiftsDTO
    {
        public long VMSMDSH_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMSMDSH_ShiftName { get; set; }
        public string VMSMDSH_ShiftStartTime { get; set; }       
        public string VMSMDSH_ShiftEndTime { get; set; }
        public bool VMSMDSH_OnlyOnSpecificDayFlg { get; set; }
        public string VMSMDSH_Remarks { get; set; }
        public bool VMSMDSH_ActiveFlag { get; set; }

        public DateTime VMSMDSH_CreatedDate { get; set; }
        public DateTime VMSMDSH_UpdatedDate { get; set; }
        public long VMSMDSH_CreatedBy { get; set; }
        public long VMSMDSH_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public Array drivers_array { get; set; }
        public Array edit_array { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }


    }
}
