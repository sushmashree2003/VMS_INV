using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Master_BudgetHeads_Amount")]
    public class VMS_Master_BudgetHeads_AmountDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMBDGHDAMT_Id { get; set; }
        public long VMBDGHD_Id { get; set; }
        public long IMFY_ID { get; set; }
        public decimal VMBDGHDAMT_Amount { get; set; }
        public decimal? VMBDGHDAMT_MonthwiseAmount { get; set; }
        public string VMBDGHDAMT_Remarks { get; set; }
        public bool VMBDGHDAMT_ActiveFlg { get; set; }
        public long VMBDGHDAMT_CreatedBy { get; set; }
        public DateTime? VMBDGHDAMT_CreatedDate { get; set; }
        public long VMBDGHDAMT_UpdatedBy { get; set; }
        public DateTime? VMBDGHDAMT_UpdatedDate { get; set; }
    }
}
