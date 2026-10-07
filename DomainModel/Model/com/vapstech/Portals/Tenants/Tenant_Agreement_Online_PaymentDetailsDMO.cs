using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Portals.Tenants
{
    [Table("Tenant_Agreement_Online_PaymentDetails")]
    public class Tenant_Agreement_Online_PaymentDetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TENANTAGRONLPAY_Id { get; set; }
        public long TENANTAGR_Id { get; set; }
        public long TENANTAGRONLPAY_PaymentMonth { get; set; }
        public Decimal TENANTAGRONLPAY_Amount { get; set; }
        public DateTime? TENANTAGRONLPAY_PaymentDate { get; set; }
        public string TENANTAGRONLPAY_Remarks { get; set; }
        public string TENANTAGRONLPAY_TransactionReferenceId { get; set; }
        public string TENANTAGRONLPAY_PaymentStatus { get; set; }
        public bool TENANTAGRONLPAY_ActiveFlg { get; set; }        
        public long TENANTAGRONLPAY_PaymentYear { get; set; }
        public DateTime TENANTAGRONLPAY_CreatedDate { get; set; }
        public DateTime TENANTAGRONLPAY_UpdatedDate { get; set; }
        public long TENANTAGRONLPAY_CreatedBy { get; set; }
        public long TENANTAGRONLPAY_UpdatedBy { get; set; }
        public string TENANTAGRONLPAY_Flag { get; set; }
        


    }
}
