using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_TaskEnhancement_Deptwise")]
    public class ISM_TaskEnhancement_DeptwiseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTEDDP_ID { get; set; }
        public long MI_ID { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public int TTEDDP_Percentage { get; set; }
        public string TTEDDP_Remarks { get; set; }
        public long TTEDDP_CreatedBy { get; set; }
        public long TTEDDP_UpdatedBy { get; set; }
        public string TTEDDP_Days { get; set; }
        public long TTEDDP_level { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? TTEDDP_FromDate { get; set; }
        public DateTime? TTEDDP_ToDate { get; set; }
        public string TTEDDP_Hours { get; set; }
    }
}
