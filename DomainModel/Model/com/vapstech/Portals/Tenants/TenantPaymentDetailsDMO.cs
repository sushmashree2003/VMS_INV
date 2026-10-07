using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Portals.Tenants
{
    [Table("Tenant_Agreement_PaymentDetails")]
    public class TenantPaymentDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTAGRPAY_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public Decimal TENANTAGRPAY_Amount { get; set; }
        public DateTime? TENANTAGRPAY_PaymentDate { get; set; }
        public long TENANTAGRPAY_ModeOfPayment { get; set; }
        public string TENANTAGRPAY_Remarks { get; set; }

        public decimal TENANTAGRPAY_PaymentMonth { get; set; }
        public long TENANTAGRPAY_PaymentYear { get; set; }

        public DateTime? TENANTAGRPAY_CreatedDate { get; set; }
        public DateTime? TENANTAGRPAY_UpdatedDate { get; set; }
        public long TENANTAGRPAY_CreatedBy { get; set; }
        public long TENANTAGRPAY_UpdatedBy { get; set; }

        public string TENANTAGRPAY_Bankname { get; set; }
        public string TENANTAGRPAY_Chequeno { get; set; }
        public string TENANTAGRPAY_ReceiptNo { get; set; }
        public DateTime? TENANTAGRPAY_Chequedate { get; set; }
        public string TENANTAGRPAY_PaymentReferenceId { get; set; }
        public long TENANTAGRONLPAY_Id { get; set; }
        public string TENANTAGRPAY_Flag { get; set; }

        public decimal? TENANTAGRPAY_Balance { get; set; }
        public decimal? TENANTAGRPAY_ActualFineAmount { get; set; }

        public decimal? TENANTAGRPAY_FinePaidAmount { get; set; }
    }
}
