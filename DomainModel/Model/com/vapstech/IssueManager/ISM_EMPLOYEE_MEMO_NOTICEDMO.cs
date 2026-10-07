using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_EMPLOYEE_MEMO_NOTICE")]
    public class ISM_EMPLOYEE_MEMO_NOTICEDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMEMN_ID { get; set; }
        public long HRME_ID { get; set; }
        public long MI_ID { get; set; }

        public long ASMAY_ID { get; set; }
        public string ISMEMN_No { get; set; }
        public string ISMEMN_Type { get; set; }
        public string ISMEMN_Description { get; set; }
        public DateTime ISMEMN_Date { get; set; }
        public long ISMEMN_CreatedBy { get; set; }
        public bool ISMEMN_EmailSentFlag { get; set; }
        public long ISMEMN_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate  { get; set; }
        public DateTime? ISMEMN_CompleByDate { get; set; }
        public DateTime? ISMEMN_Startdate { get; set; }
        public DateTime? ISMEMN_Enddate { get; set; }
    }
}
