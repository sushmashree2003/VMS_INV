using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("VMS_Master_BudgetHeads_Amount_Month")]
    public class VMS_Master_BudgetHeads_Amount_MonthDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long VMBDGHDAMTNM_Id { get; set; }
        public long VMBDGHDAMTN_Id { get; set; }
        public long VMBDGHDAMTNM_MonthId { get; set; }
        public bool VMBDGHDAMTNM_ActiveFlag { get; set; }
        public long VMBDGHDAMTNM_CreatedBy { get; set; }
        public long VMBDGHDAMTNM_UpdatedBy { get; set; }
        public DateTime? VMBDGHDAMTNM_CreatedDate { get; set; }
        public DateTime? VMBDGHDAMTNM_UpdatedDate { get; set; }
        public decimal VMBDGHDAMTNM_Amount { get; set; }
    }
}
