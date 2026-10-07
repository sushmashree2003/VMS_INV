using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
     public class CheckBookLogReportDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array allcompany { get; set; }
        public Array banklist { get; set; }
        public Array Cheque { get; set; }
        public long VPAYVOU_Id { get; set; }
        public string type { get; set; }
        public Array alldata { get; set; }
        public long HRMBD_Id { get; set; }
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public DateTime? VPAYVOU_PaymentDate { get; set; }
        public chequelist[] chequedetails { get; set; }
        public banklist[] Banklistdetails { get; set; }

        public string MI_Logo { get; set; }
        public Array allReport { get; set; }
       
    }
    public class chequelist
    {
        public long VPAYVOU_Id { get; set; }

    }
   public class banklist
    {
        public long HRMD_Id { get; set; }
    }


}
