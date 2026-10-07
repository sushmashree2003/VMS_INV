using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory.StatutoryTaxCompliance
{
    [Table("VMS_Statutory_Compliance")]
    public  class VMS_Statutory_ComplianceDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   

        public long VMSSC_Id { get; set; }
        public long MI_Id { get; set; }
        public long IMFY_Id { get; set; }
        public string VMSSC_Flag { get; set; }
        public string VMSSC_ReferenceNo { get; set; }
        public string VMSSC_Remarks { get; set; }
        public DateTime VMSSC_FromDate { get; set; }
        public DateTime VMSSC_ToDate { get; set; }
        public string VMSSC_OtherRemarks { get; set; }
        public bool VMSSC_CaseFlag { get; set; }
        public string VMSSC_CaseDtails { get; set; }
        public DateTime VMSSC_CreatedDate { get; set; }
        public DateTime VMSSC_UpdatedDate { get; set; }
        public long VMSSC_CreatedBy { get; set; }
        public long VMSSC_UpdatedBy { get; set; }
        public string VMSSC_Status { get; set; }
        public List<VMS_Statutory_Compliance_DocumentsDMO> VMS_Statutory_Compliance_DocumentsDMO { get; set; }
        public List<VMS_Statutory_consultantDMO> VMS_Statutory_consultantDMO { get; set; }
    }
}
