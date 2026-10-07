using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_DailyReport_FeedBack_Transaction_DTO
    {
        public long ISMDRFT_Id { get; set; }
        public long ISMDRF_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMDRFT_Send_HRME_Id { get; set; }
        public long ISMDRFT_RCV_HRME_Id { get; set; }
        public string ISMDRFT_Feedback { get; set; }
        public long ISMDRFT_Status { get; set; }
        public bool ISMDRFT_Openflg { get; set; }
        public DateTime createdate { get; set; }
        public DateTime updatedate { get; set; }
    }
}
