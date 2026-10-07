using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.IssueManager
{
    [Table("ISM_DailyReport_FeedBack")]
    public class ISM_DailyReport_FeedBack_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMDRF_Id { get; set; }
        public long ISMDRF_Send_HRME_Id { get; set; }
        public long ISMDRF_RCV_HRME_Id { get; set; }
        public string ISMDRF_FeedBack { get; set; }
        public bool ISMDRF_Flg { get; set; }
        public bool? ISMDRF_OpenFlg { get; set; }
        public bool? ISMDRF_OpenFeedback { get; set; }
        public long ISMTCR_Id { get; set; }
        public DateTime? ISMDRF_Feedback_DR_Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<ISM_DailyReport_Feedback_Send_DMO> ISM_DailyReport_Feedback_Send_DMO { get; set; }
        public List<ISM_DailyReport_Feedback_Receive_DMO> ISM_DailyReport_Feedback_Receive_DMO { get; set; }
    }
}
