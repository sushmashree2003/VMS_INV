using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_DailyReport_Feedback_Send_DTO
    {
        public long ISMDRFS_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMDRF_Id { get; set; }
        public long ISMDRFS_Send_HRME_Id { get; set; }
        public string ISMDRFS_FeedBack { get; set; }
        public string ISMDRFS_Status { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatetime { get; set; }
    }
}
