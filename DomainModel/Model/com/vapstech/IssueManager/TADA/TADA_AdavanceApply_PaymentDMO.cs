using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_TADA_Advance_Apply_Payment")]
    public  class TADA_AdavanceApply_PaymentDMO
    { 
       [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long VTADAAAP_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public string VTADAAAP_PaymentMode { get; set; }
        public decimal VTADAAAP_AmountPaid { get; set; }
        public string VTADAAAP_PaymentRefNo { get; set; }
        public string VTADAAAP_Remarks { get; set; }
        public DateTime? VTADAAAP_Date { get; set; }
        public bool VTADAAAP_ActiveFlg { get; set; }
        public long VTADAAAP_CreatedBy { get; set; }
        public DateTime? VTADAAAP_CreatedDate { get; set; }

        public long VTADAAAP_UpdatedBy { get; set; }
        public DateTime VTADAAAP_UpdatedDate { get; set; }

    }
}
