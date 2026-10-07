using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.SpaceBooking
{
    public class SBReport_DTO : CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long MI_Id_new { get; set; }
        public long miid { get; set; }
        public long UserId { get; set; }
        public long HRMR_Id { get; set; }
        public DateTime? fromdate1 { get; set; }
        public DateTime? fromdate2 { get; set; }

        public string userflag { get; set; }
        public string enqstatus { get; set; }
        public string enqtype { get; set; }
        public string bookingtype { get; set; }
        public string bookingstatus { get; set; }
        public string company_name { get; set; }
        //public string HRMR_RoomName { get; set; }
        public SBReport_DTO[] selectedroomlist { get; set; }
        public MI_IDLISt[] MI_IDList { get; set; }
        
        public Array userReg_Report { get; set; }
        public Array enqlist_Report { get; set; }
        public Array bookinglist_Report { get; set; }
        public Array roomname_list { get; set; }
        public Array milist { get; set; }
        public string HRMR_RoomName { get; set; }
        public Array Master_Institution { get; set; }
    }
    
    public class MI_IDLISt
    {
        public long MI_IDS { get; set; }
    }
}
