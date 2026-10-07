using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee.Tally
{
    [Table("Tally_M_Transaction")]
   public class TallyMTransactionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TMT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime TMT_Date { get; set; }
        public bool TMT_VoucherTypeFlg { get; set; }
        public long TMT_VoucherNo { get; set; }
        public decimal TMT_Amount { get; set; }
        public bool TMT_TransactionStatusFlg { get; set; }
        public bool TMT_TransactionTypeFlg { get; set; }
        public bool TMT_APIStatusFlg { get; set; }
        public string TMT_ChequeNo { get; set; }
        public long TMT_ExportToTallyFlg { get; set; }
        public DateTime TMT_ChequeDate { get; set; }
        public long TMT_RefNo { get; set; }
        public int TMT_FinancialYear { get; set; }

        public bool TMT_ActiveFlg { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
