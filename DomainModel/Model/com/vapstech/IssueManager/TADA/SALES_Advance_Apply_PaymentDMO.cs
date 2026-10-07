using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager.TADA
{
    [Table("VMS_SALES_Advance_Apply_Payment")]
    public  class SALES_Advance_Apply_PaymentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VTADASLEAP_Id { get; set; }
        public long VTADAAA_Id { get; set; }
        public string VTADASLEAP_PaymentMode { get; set; }
        public decimal? VTADASLEAP_AmountPaid { get; set; }
        public string VTADASLEAP_PaymentRefNo { get; set; }
        public string VTADASLEAP_Remarks { get; set; }
        public DateTime? VTADASLEAP_Date { get; set; }
        public bool VTADASLEAP_ActiveFlg { get; set; }
        public long VTADASLEAP_CreatedBy { get; set; }
        public long VTADASLEAP_UpdatedBy { get; set; }
        public DateTime? VTADASLEAP_UpdatedDate { get; set; }
        public DateTime? VTADASLEAP_CreatedDate { get; set; }
    }
}
