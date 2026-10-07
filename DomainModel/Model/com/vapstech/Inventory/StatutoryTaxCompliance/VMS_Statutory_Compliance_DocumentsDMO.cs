using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory.StatutoryTaxCompliance
{
    [Table("VMS_Statutory_Compliance_Documents")]
    public class VMS_Statutory_Compliance_DocumentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSSCD_Id { get; set; }
        public long VMSSC_Id { get; set; }
        public string VMSSCD_Filename { get; set; }
        public string VMSSCD_Filepath { get; set; }
        public bool VMSSCD_ActiveFlag { get; set; }
        public DateTime VMSSCD_CreatedDate { get; set; }
        public DateTime VMSSCD_UpdatedDate { get; set; }
        public long VMSSCD_CreatedBy { get; set; }
        public long VMSSCD_UpdatedBy { get; set; }
       
    }
}
