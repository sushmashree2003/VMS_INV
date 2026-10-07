using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_Feedback_Send")]
    public class ISM_DailyReport_Feedback_Send_DMO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public long ISMDRFS_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMDRF_Id { get; set; }
        public long ISMDRFS_Send_HRME_Id { get; set; }
        public string ISMDRFS_FeedBack { get; set; }
        public string ISMDRFS_Status { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatetime { get; set; }
    }
}
