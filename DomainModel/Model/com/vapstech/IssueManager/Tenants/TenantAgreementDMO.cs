using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager.Tenants
{
    [Table("Tenant_Agreement")]
    public class TenantAgreement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long TENANTAGR_Id { get; set; }
        public long MI_Id { get; set; }
        public string TENANTAGR_TenantName { get; set; }
        public long TENANTAGR_NoOfOccupants { get; set; }
        public string TENANTAGR_Address { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMR_Id { get; set; }
        public string TENANTAGR_EmailId { get; set; }
        public string TENANTAGR_PANCardNo { get; set; }
        public string TENANTAGR_ContactNo { get; set; }
        public decimal TENANTAGR_AgreementDuration { get; set; }
        public DateTime TENANTAGR_AgrFromDate { get; set; }
        public DateTime TENANTAGR_AgrToDate { get; set; }
        public Decimal TENANTAGR_AdvanceAmount { get; set; }
        public Decimal TENANTAGR_MonthlyRent { get; set; }
        public string TENANTAGR_RentDueDay { get; set; }
        //public string  TENANTAGR_RentDueMonth { get;set;}
        public bool TENANTAGR_RenewedFlag { get; set; }
        public bool TENANTAGR_ActiveFlg { get; set; }
        public string TENANTAGR_Remarks { get; set; }
        public Decimal? TENANTAGR_RenewalPercentage { get; set; }
        public long TENANTAGR_UserId { get; set; }

        public bool TENANTAGR_LeftFlg { get; set; }
        public string TENANTAGR_BySource { get; set; }


       public long? TENANTAGR_Aadhar { get; set; }
       public string TENANTAGR_Imagepath { get; set; }

        public string TENANTAGR_LeftRemarks { get; set; }
        public DateTime? TENANTAGR_CreatedDate { get; set; }
        public DateTime? TENANTAGR_UpdatedDate { get; set; }
        public long? TENANTAGR_CreatedBy { get; set; }
        public long? TENANTAGR_UpdatedBy { get; set; }
        public decimal? TENANTAGR_RefundedAmount { get; set; }
        public string TENANTAGR_RefundModeOfPayment { get; set; }
        public List<TenantAgreementFiles> TenantAgreementFiles { get; set; }
        public List<TenantAgreementRenewal> TenantAgreementRenewal { get; set; }



    }
}
