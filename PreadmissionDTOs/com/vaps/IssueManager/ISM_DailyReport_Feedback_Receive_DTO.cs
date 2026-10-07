using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_DailyReport_Feedback_Receive_DTO
    {
        public long ISMDRFR_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMDRF_Id { get; set; }
        public long ISMDRFR_Receive_HRME_Id { get; set; }
        public string ISMDRFR_FeedBack { get; set; }
        public string ISMDRFR_Status { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatetime { get; set; }
    }
}
