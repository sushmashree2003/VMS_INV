using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory.StatutoryTaxCompliance
{
    [Table("VMS_Statutory_consultant")]
    public class VMS_Statutory_consultantDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMSSCT_Id { get; set; }
        public long VMSSC_Id { get; set; }
        public string VMSSCT_consultantName { get; set; }
        public string VMSSCT_Address { get; set; }
        public string VMSSCT_Remarks { get; set; }
        public bool VMSSCT_ActiveFlag { get; set; }
        public DateTime VMSSCT_CreatedDate { get; set; }
        public DateTime VMSSCT_UpdatedDate { get; set; }
        public long VMSSCT_CreatedBy { get; set; }
        public long VMSSCT_UpdatedBy { get; set; }
        public bool VMSSCT_Flag { get; set; }


    }
}
