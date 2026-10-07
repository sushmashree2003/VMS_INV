using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{

    [Table("VMS_TADA_Apply_Payment")]
    public class TADAApply_PaymentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADAAP_Id { get; set; }
        
        public long VTADAA_Id { get; set; }
        public string VTADAAP_PaymentMode { get; set; }
        public decimal VTADAAP_AmountPaid { get; set; }
        public string VTADAAP_PaymentRefNo { get; set; }
        public string VTADAAP_Remarks { get; set; }
        public DateTime? VTADAAP_Date { get; set; }
        public bool VTADAAP_ActiveFlg { get; set; }
        public long VTADAAP_CreatedBy { get; set; }
        public DateTime? VTADAAP_CreatedDate { get; set; }
        public long VTADAAP_UpdatedBy { get; set; }
        public DateTime? VTADAAP_UpdatedDate { get; set; }

    }
}
