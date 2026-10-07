using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{

    [Table("VMS_Master_BudgetHeads_Amount_new")]
    public class VMS_Master_BudgetHeads_Amount_newDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMBDGHDAMTN_Id { get; set; }
        public long VMBDGHDN_Id { get; set; }
        public long IMFY_ID { get; set; }
        public long? HRMD_Id { get; set; }
        public decimal VMBDGHDAMTN_Amount { get; set; }
        public decimal? VMBDGHDAMTN_MonthwiseAmount { get; set; }
        public string VMBDGHDAMTN_Remarks { get; set; }
        public string VMBDGHDAMTN_Name { get; set; }
        public bool VMBDGHDAMTN_ActiveFlg { get; set; }
        public long VMBDGHDAMTN_CreatedBy { get; set; }
        public DateTime? VMBDGHDAMTN_CreatedDate { get; set; }
        public long VMBDGHDAMTN_UpdatedBy { get; set; }
        public DateTime? VMBDGHDAMTN_UpdatedDate { get; set; }
        public long? ISMMCLT_Id { get; set; }
        public List<VMS_Master_BudgetHeads_Amount_MonthDMO> VMS_Master_BudgetHeads_Amount_MonthDMO { get; set; }


    }
}
