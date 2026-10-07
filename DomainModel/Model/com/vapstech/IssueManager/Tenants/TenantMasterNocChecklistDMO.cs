using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_NOC_Checklist")]
    public class TenantMasterNocChecklistDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    
        public long TENANTNOCCL_Id { get; set; }
        public long  TENANTAGR_Id {get;set;}
        public long HRME_Id { get;set;}
        public long HRMAM_Id { get;set;}
        public Boolean TENANTNOCCL_WorkingFlg { get;set;}
        public Boolean TENANTNOCCL_ActiveFlag { get;set;}
        public DateTime TENANTNOCCL_CheckedDate { get;set;}
        public string TENANTNOCCL_Remarks { get;set;}
        public decimal TENANTNOCCL_DeductingAmount { get;set;}
        public DateTime TENANTNOCCL_CreatedDate { get;set;}
        public DateTime TENANTNOCCL_UpdatedDate { get;set;}
        public long TENANTNOCCL_CreatedBy { get;set;}
        public long TENANTNOCCL_UpdatedBy { get;set;}
        public string TENANTNOCCL_FileName { get; set; }
        public string TENANTNOCCL_FilePath { get; set; }
    }
}
