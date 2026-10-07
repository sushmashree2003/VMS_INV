using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Printer_Dept_Mapping")]
    public class PrinterDeptMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSPDM_Id { get; set; }
        public long VMSMPR_Id { get; set; }
        public long HRMD_Id { get; set; }
        public bool VMSPDM_ActiveFlag { get; set; }
        public DateTime? VMSPDM_CreatedDate { get; set; }
        public DateTime? VMSPDM_UpdatedDate { get; set; }
        public long? VMSPDM_Createdby { get; set; }
        public long? VMSPDM_Updatedby { get; set; }


    }
}
