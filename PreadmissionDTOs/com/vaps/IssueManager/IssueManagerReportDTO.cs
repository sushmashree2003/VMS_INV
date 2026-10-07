using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class IssueManagerReportDTO
    {
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
       public Array getReport { get; set; }
        public string optionflag { get; set; }
        public Array floorList { get; set; }
      public HRMF_FloorNameLIST[] HRMF_FloorNamelist { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string FloarName { get; set; }
    }
    public class HRMF_FloorNameLIST
    {
        public string hrmF_FloorName { get; set; }
    }
}
