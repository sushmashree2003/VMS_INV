using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Master_BudgetHeads")]
    public class VMS_Master_BudgetHeadsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMBDGHD_Id { get; set; }
        public long MI_Id { get; set; }
        public string VMBDGHD_BudgetHead { get; set; }
        public bool VMBDGHD_MonthwiseBudgetFlg { get; set; }
        public string VMBDGHD_Remarks { get; set; }
        public bool VMBDGHD_ActiveFlg { get; set; }
        public long VMBDGHD_CreatedBy { get; set; }
        public DateTime? VMBDGHD_CreatedDate { get; set; }
        public long VMBDGHD_UpdatedBy { get; set; }
        public DateTime? VMBDGHD_UpdatedDate { get; set; }
    }
}
