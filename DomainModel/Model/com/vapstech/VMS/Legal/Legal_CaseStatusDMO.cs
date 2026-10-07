using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.Legal
{
    [Table("Legal_CaseStatus")]
    public class Legal_CaseStatusDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LCS_Id { get; set; }
        public string LCS_CaseStatus { get; set; }
        public long LCD_Id { get; set; }
        public DateTime? LCS_FirstHearingDate { get; set; }
        public DateTime? LCS_DecisionDate { get; set; }
        public string LCS_NatureofDisposal { get; set; }
        public string LCS_CourtNumberandJudge { get; set; }
        public bool LCS_ActiveFlag { get; set; }
        public DateTime? LCS_CreatedDate { get; set; }
        public DateTime? LCS_UpdatedDate { get; set; }
        public long LCS_CreatedBy { get; set; }
        public long LCS_UpdatedBy { get; set; }
    }
}
