using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
   public class ApprovalInOutStatus_DTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public Array institutionlist { get; set; }
        public Array visitorlist { get; set; }
        public Array editDetails { get; set; }
        public string VMAP_ChekInOutStatus { get; set; }
        public long VMAP_Id { get; set; }
        public string VMAP_VisitorName { get; set; }
        public string VMAP_ExitDateTime { get; set; }
        public string VMAP_Remarks { get; set; }
        public string returnVal { get; set; }
        public string type { get; set; }
        public string empname { get; set; }
        public DateTime? VMAP_EntryDateTime { get; set; }
        public DateTime? VMAP_MeetingDateTime { get; set; }
    }
}