using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReportExtension")]
    public class ISM_DailyReportExtensionDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMODE_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IVRM_Month_Id { get; set; }
        public string ISMODE_Noofdays { get; set; }       
        public DateTime ISMODE_Date { get; set; }
        public string ISMODE_Reason { get; set; }
        public bool ISMODE_ActiveFlg { get; set; }
        public long ISMODE_CreatedBy { get; set; }
        public long ISMODE_UpdatedBy { get; set; }

    }
}
