using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class AppointmentStatusDTO
    {       

        public long VMMV_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMMV_VisitorName { get; set; }
        public DateTime VMMV_MeetingDateTime { get; set; }
        public string VMMV_CkeckedInOutStatus { get; set; }
        public string VMMV_EntryDateTime { get; set; }
        public string VMMV_ExitDateTime { get; set; }
        public string VMMV_Remarks { get; set; }
        public long? VMMV_UpdatedBy { get; set; }
        public long UserId { get; set; }

        public Array visitorlist { get; set; }
        public Array editDetails { get; set; }
        public string returnVal { get; set; }
        public Array vis_list { get; set; }
        public string empname { get; set; }
        public string type { get; set; }

        public Array institution { get; set; }
        public Array institutionlist { get; set; }

    }
}
