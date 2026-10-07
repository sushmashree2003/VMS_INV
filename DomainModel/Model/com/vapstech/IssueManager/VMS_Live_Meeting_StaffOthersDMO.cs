using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Live_Meeting_StaffOthers")]
    public class VMS_Live_Meeting_StaffOthersDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSLMEETSTFOTH_Id { get; set; }
        public long VMSLMEET_Id { get; set; }
        public long User_Id { get; set; }
        public long HRME_Id { get; set; }
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
