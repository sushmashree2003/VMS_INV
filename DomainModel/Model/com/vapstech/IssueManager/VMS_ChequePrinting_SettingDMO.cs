using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_ChequePrinting_Setting")]
    public class VMS_ChequePrinting_SettingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VCPSET_Id { get; set; }
        public long HRMBD_Id { get; set; }
        public string VCPSET_PrintingTemplateId { get; set; }
        public string VCPSET_ChequeImageFileName { get; set; }
        public string VCPSET_ChequeImageFilePath { get; set; }
        public bool VCPSET_DefaultFlg { get; set; }
        public string VCPSET_Remarks { get; set; }
        public bool VCPSET_ActiveFlg { get; set; }
        public long VCPSET_CreatedBy { get; set; }
        public long VCPSET_UpdatedBy { get; set; }
        public DateTime? VCPSET_CreatedDate { get; set; }
        public DateTime? VCPSET_UpdatedDate { get; set; }
        
    }
}
