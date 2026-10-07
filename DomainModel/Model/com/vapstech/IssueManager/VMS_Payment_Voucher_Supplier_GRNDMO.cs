using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Payment_Voucher_Supplier_GRN")]
    public class VMS_Payment_Voucher_Supplier_GRNDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VPAYVOUSGRN_Id { get; set; }
        public long VPAYVOU_Id { get; set; }
        public long INVMS_Id { get; set; }
        public long INVMGRN_Id { get; set; }
        public decimal? VPAYVOUSGRN_ActualTotalAmount { get; set; }
        public decimal? VPAYVOUSGRN_PaidAmount { get; set; }
        public decimal? VPAYVOUSGRN_BalanceAmount { get; set; }
        public string VPAYVOUSGRN_Remarks { get; set; }
        public bool VPAYVOUSGRN_ActiveFlg { get; set; }
        public long VPAYVOUSGRN_CreatedBy { get; set; }
        public DateTime? VPAYVOUSGRN_CreatedDate { get; set; }
        public long VPAYVOUSGRN_UpdatedBy { get; set; }
        public DateTime? VPAYVOUSGRN_UpdatedDate { get; set; }


    }
}
