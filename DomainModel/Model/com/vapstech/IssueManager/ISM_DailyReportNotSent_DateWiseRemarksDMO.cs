using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReportNotSent_DateWiseRemarks")]
    public class ISM_DailyReportNotSent_DateWiseRemarksDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDRNSDR_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? ISMDRNSDR_DRNotSentDate { get; set; }
        public DateTime? ISMDRNSDR_RemarksDate { get; set; }
        public string ISMDRNSDR_Remarks { get; set; }
        public string ISMDRNSDR_TemplateName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
