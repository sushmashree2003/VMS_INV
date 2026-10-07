using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class VMS_Live_MeetingDTO
    {
        public long VMSLMEET_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string VMSLMEET_MeetingId { get; set; }
        public long User_Id { get; set; }
        public long HRME_Id { get; set; }
        public string VMSLMEET_MeetingTopic { get; set; }
        public string VMSLMEET_MeetingURL { get; set; }
        public string VMSLMEET_PIPAddress { get; set; }
        public string VMSLMEET_PMACAddress { get; set; }
        public DateTime? VMSLMEET_PlannedDate { get; set; }
        public string VMSLMEET_PlannedStartTime { get; set; }
        public string VMSLMEET_PlannedEndTime { get; set; }
        public DateTime? VMSLMEET_MeetingDate { get; set; }
        public string VMSLMEET_StartedTime { get; set; }
        public string VMSLMEET_EndTime { get; set; }
        public string VMSLMEET_IPAddress { get; set; }
        public string VMSLMEET_MACAddress { get; set; }
        public bool VMSLMEET_ActiveFlg { get; set; }
        public DateTime VMSLMEET_CreatedDate { get; set; }
        public DateTime VMSLMEET_UpdatedDate { get; set; }
        public long VMSLMEET_CreatedBy { get; set; }
        public long VMSLMEET_UpdatedBy { get; set; }
        public bool LMSLMEET_Recordflag { get; set; }
        public string LMSLMEET_RecordId { get; set; }
        public string LMSLMEET_internalMeetingID { get; set; }
        public string orderid { get; set; }
        public string message { get; set; }
        public string vcflag { get; set; }
        public bool returnval { get; set; }
        public string moderatorurl { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Array get_userEmplist { get; set; }
        public Array meetinglist { get; set; }
        public Array meetingstart { get; set; }
        public Array vmslivemeetinglist { get; set; }
        public Array vmslivemeetingreport { get; set; }
        public arrayuserEmp1[] arrayuserEmp { get; set; }
        public class arrayuserEmp1
        {
            public long HRME_Id { get; set; }
        }
    }
}
