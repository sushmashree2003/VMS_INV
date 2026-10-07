using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_BANK_ACCOUNT_BALANCE")]
    public class VMS_BANK_ACCOUNT_BALANCEDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VBABAL_Id { get; set; }
        public decimal? VBABAL_ActualBankBalance { get; set; }
        public decimal? VBABAL_BalanceAsperVoucher { get; set; }
        public long VBABAL_UpdatedBy { get; set; }
        public bool VBABAL_ActiveFlg { get; set; }
        public long VBABAL_CreatedBy { get; set; }
        public DateTime? VBABAL_CreatedDate { get; set; }
        public  DateTime? VBABAL_UpdatedDate { get; set; }
        public  long MI_Id { get; set; }
        public long HRMBD_Id { get; set; }
        public string VBABAL_CRDRFlg { get; set; }
        public decimal? VBABAL_BalanceAsperVoucherReceipt { get; set; }
    }
}
