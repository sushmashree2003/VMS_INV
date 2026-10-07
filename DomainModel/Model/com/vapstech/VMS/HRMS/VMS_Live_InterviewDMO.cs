using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("VMS_Live_Interview")]
   public class VMS_Live_InterviewDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSLITVW_Id { get; set; }
        public long MI_Id { get; set; }
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
        public DateTime? VMSLMEET_CreatedDate { get; set; }
        public DateTime? VMSLMEET_UpdatedDate { get; set; }
        public long VMSLMEET_CreatedBy { get; set; }
        public long VMSLMEET_UpdatedBy { get; set; }
        public bool LMSLMEET_Recordflag { get; set; }
        public string LMSLMEET_RecordId { get; set; }
        public string LMSLMEET_internalMeetingID { get; set; }
        public List<VMS_Live_Meeting_CandidateOthersDMO> VMS_Live_Meeting_CandidateOthersDMO { get; set; }
    }
}
