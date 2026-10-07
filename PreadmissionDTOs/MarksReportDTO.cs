using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MarksReportDTO : CommonParamDTO
    {

        public Array fillsub { get; set; }
        public Array fillhead { get; set; }

        public Array fillclass { get; set; }

        public string asmayid { get; set; }
        public long asmclid { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string flagows { get; set; }
        public Array allreports { get; set; }
        public string oralwrittenscheduleflag { get; set; }
        public int  mid { get; set; }
        public Array writentestlist { get; set; }
        public long asmay_id { get; set; }
        public Array orallist { get; set; }
        public int yearid { get; set; }
        public long disid { get; set; }
        public string disname { get; set; }
        public string PASR_FirstName { get; set; }
        public string PASR_MiddleName { get; set; }
        public string PASR_LastName { get; set; }
        public string regno { get; set; }
        public long PAOTM_Id { get; set; }
        
        public decimal PAOTMS_Marks { get; set; }
        public long schids { get; set; }
    
        public long PASWMS_Id { get; set; }
        public long PASR_Id { get; set; }
        public decimal PASWMS_MarksScored { get; set; }
        public long hid { get; set; }
        public string hhead { get; set; }
        public decimal? hmaxmarks { get; set; }
        public long ISMS_Id { get; set; }

        public string classname { get; set; }
    }
}
