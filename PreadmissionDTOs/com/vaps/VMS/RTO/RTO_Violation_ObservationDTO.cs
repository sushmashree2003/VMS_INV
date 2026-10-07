using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.RTO
{
   public class RTO_Violation_ObservationDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public Array vehicalArray { get; set; }
        public Array emplist { get; set; }
        public Array fineArray { get; set; }
        public Array RToVialation { get; set; }
        public long RTOVOB_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime? RTOVOB_Date { get; set; }
        public bool? RTOVOB_AnyViolationFlg { get; set; }
        public string RTOVOB_DateTimeOfViolation { get; set; }
        public long RTOMFP_Id { get; set; }
        public string RTOVOB_ViolationDescription { get; set; }
        public decimal RTOVOB_FineAmount { get; set; }
        public bool RTOVOB_ActiveFlag { get; set; }
        public bool RTOVOB_FineAmountPaidFlg { get; set; }
        public Violation_ObservationDTO[] ViolationFiles { get; set; }
        public Array editlist { get; set; }
        public Array filist { get; set; }
        public Array RTO_VialationList { get; set; }
        public string Template { get; set; }
       public string CreatedBy { get; set; }
    }
    public class Violation_ObservationDTO
    {
        public string RTOVOBFL_FileName { get; set; }
        public string RTOVOBFL_FilePath { get; set; }
        public string RTOVOBFL_Remarks { get; set; }
    }
}
