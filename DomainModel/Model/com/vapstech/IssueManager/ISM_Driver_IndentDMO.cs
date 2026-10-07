using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_Driver_Indent")]
    public class ISM_Driver_IndentDMO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ISMDIT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? ISMDIT_Date { get; set; }
        public long TRMV_Id { get; set; }
        public string ISMDIT_BillNo { get; set; }
        public decimal ISMDIT_Qty { get; set; }
        public decimal ISMDIT_Amount { get; set; }
        public string ISMDIT_Remarks { get; set; }
        public decimal ISMDIT_OpeningKM { get; set; }
        public decimal ISMDIT_ClosingKM { get; set; }
        public decimal ISMDIT_BalanceDiesel { get; set; }
        public decimal ISMDIT_PreviousReading { get; set; }
        public decimal ISMDIT_CurrentReading { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
