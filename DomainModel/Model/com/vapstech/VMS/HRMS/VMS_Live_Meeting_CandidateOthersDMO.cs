using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("VMS_Live_Meeting_CandidateOthers")]
    public class VMS_Live_Meeting_CandidateOthersDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSLMEETCDTOTH_Id { get; set; }
        public long VMSLITVW_Id { get; set; }
        public long User_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string VMSLMEETSTFOTH_LoginTime { get; set; }
        public string VMSLMEETSTFOTH_MACAddress { get; set; }
        public string VMSLMEETSTFOTH_IPAddress { get; set; }
        public string VMSLMEETSTFOTH_LogoutTime { get; set; }
        public bool VMSLMEETSTFOTH_ActiveFlg { get; set; }
        public DateTime VMSLMEETSTFOTH_CreatedDate { get; set; }
        public DateTime VMSLMEETSTFOTH_UpdatedDate { get; set; }
        public long VMSLMEETSTFOTH_CreatedBy { get; set; }
        public long VMSLMEETSTFOTH_UpdatedBy { get; set; }
    }
}
