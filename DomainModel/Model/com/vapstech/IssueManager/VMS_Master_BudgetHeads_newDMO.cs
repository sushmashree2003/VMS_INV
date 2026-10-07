using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{

    [Table("VMS_Master_BudgetHeads_new")]
    public class VMS_Master_BudgetHeads_newDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VMBDGHDN_Id { get; set; }
        public long MI_Id { get; set; }
        public long? HRMD_Id { get; set; }
        public string VMBDGHDN_BudgetHead { get; set; }
        public bool VMBDGHDN_MonthwiseBudgetFlg { get; set; }
        public string VMBDGHDN_Remarks { get; set; }
        public bool VMBDGHDN_ActiveFlg { get; set; }
        public long VMBDGHDN_CreatedBy { get; set; }
        public DateTime VMBDGHDN_CreatedDate { get; set; }
        public long VMBDGHDN_UpdatedBy { get; set; }
        public DateTime VMBDGHDN_UpdatedDate { get; set; }
        public string VMBDGHDN_SatuswiseFlg { get; set; }
        public long? ISMMCLT_Id { get; set; }
    }
}
