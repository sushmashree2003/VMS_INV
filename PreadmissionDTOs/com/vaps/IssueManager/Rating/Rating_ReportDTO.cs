using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager.Rating
{
  public class Rating_ReportDTO
    {
      public long  RATMTQO_Id { get; set; }
        public long RATMTQ_Id { get; set; }
        public long RATMOP_Id { get; set; }

        public long RATMTQO_CreatedBy { get; set; }
        public long RATMTQO_UpadatedBy { get; set; }
        public DateTime? RATMTQO_CreatedDate { get; set; }
        public DateTime? RATMTQO_UpadatedDate { get; set; }

        public long RATMTQO_TOOOrder { get; set; }
        public long RATMTQO_ActiveFlag { get; set; }

        public string FlagOnLoadOrOnChangeInst { get; set; }

        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array getuserinstitution { get; set; }
        public Array GetMasterTypeLoadData { get; set; }
        public Array GetMasterQuestionLoadData { get; set; }
        public Array GetMasterQuestionOrderData { get; set; }
        public Array getmastertype { get; set; }
        public Array details { get; set; }

        public string RATMQE_RatingQuestions { get; set; }
        public string RATMOP_RatingOptions { get; set; }
        public long RATMT_Id { get; set; }
        public long RATMQE_Id { get; set; }
        public Array get_report { get; set; }
        public string deptName { get; set; }
        public Array deptdetails { get; set; }
}
}
