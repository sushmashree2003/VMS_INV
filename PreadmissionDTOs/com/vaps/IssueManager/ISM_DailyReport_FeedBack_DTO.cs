using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
    public class ISM_DailyReport_FeedBack_DTO
    {
        public long ISMDRF_Id { get; set; }
        public long ISMDRF_Send_HRME_Id { get; set; }
        public long ISMDRF_RCV_HRME_Id { get; set; }
        public string ISMDRF_FeedBack { get; set; }
        public bool ISMDRF_Flg { get; set; }
        public bool ISMDRF_OpenFlg { get; set; }
        public long ISMTCR_Id { get; set; }
       
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
