using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Master_Printer")]
    public class PrinterDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VMSMPR_Id { get; set; }
        public string VMSMPR_Name { get; set; }
        public string VMSMPR_PSerialNo { get; set; }
        public bool VMSMPR_ActiveFlag { get; set; }
        public DateTime? VMSMPR_CreatedDate { get; set; }
        public DateTime? VMSMPR_UpdatedDate{ get; set; }
        public long? VMSMPR_Createdby { get; set; }
        public long? VMSMPR_Updatedby { get; set; }

}
}
