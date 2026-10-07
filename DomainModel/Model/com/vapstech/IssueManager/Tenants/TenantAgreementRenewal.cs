using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_Agreement_Renewals")]
    public class TenantAgreementRenewal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTAGRREN_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public DateTime TENANTAGRREN_PreAgrFromDate { get; set; }
        public DateTime TENANTAGRREN_PreAgrToDate { get; set; }
        public string TENANTAGRREN_Remarks { get; set; }

        public bool TENANTAGRREN_ActiveFlag { get; set; }
        public DateTime? TENANTAGRREN_CreatedDate { get; set; }
        public DateTime? TENANTAGRREN_UpdatedDate { get; set; }
        public long? TENANTAGRREN_CreatedBy { get; set; }
        public long? TENANTAGRREN_UpdatedBy { get; set; }
        public int? TENANTAGRREN_AggreedEnhancement { get; set; }
        public string TENANTAGRREN_AmtOrPercentFlg { get; set; }
        public long TENANTAGRREN_Rent { get; set; }



    }
}
