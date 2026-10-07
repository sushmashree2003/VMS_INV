using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Payment_Voucher_Budget")]
    public  class VMS_Payment_Voucher_BudgetDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VPAYVOUBUD_Id { get; set; }
        public long VPAYVOU_Id { get; set; }
        public long VMBDGHD_Id { get; set; }
        public decimal? VMBDGHD_AmountSpent { get; set; }
        public bool VPAYVOUBUD_ActiveFlg { get; set; }
        public long VPAYVOUBUD_CreatedBy { get; set; }
        public DateTime? VPAYVOUBUD_CreatedDate { get; set; }
        public long VPAYVOUBUD_UpdatedBy { get; set; }
        public DateTime? VPAYVOUBUD_UpdatedDate { get; set; }
        public string VPAYVOUBUD_Remark { get; set; }
    }
}
