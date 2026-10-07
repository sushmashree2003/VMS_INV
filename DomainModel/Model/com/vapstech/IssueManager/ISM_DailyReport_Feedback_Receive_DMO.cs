using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Feedback_Receive")]
   public  class ISM_DailyReport_Feedback_Receive_DMO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public long ISMDRFR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMDRF_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMDRFR_Receive_HRME_Id { get; set; }
        public string ISMDRFR_FeedBack { get; set; }
        public string ISMDRFR_Status { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatetime { get; set; }
    }
}
