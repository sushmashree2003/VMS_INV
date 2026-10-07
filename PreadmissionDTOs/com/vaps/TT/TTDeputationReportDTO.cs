using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.TT
{
   public class TTDeputationReportDTO
    {
        public long MI_ID { get; set; }
        public long ASMAY_ID { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public Array reportdatelist { get; set; }
    }
}
