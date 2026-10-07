using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.RTO
{
    public class RTO_VoilationReportDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public Array Rtoviolations { get; set; }
        public Array GetReportt { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public rto_ob_report_list[] rto_ob_report_list { get; set; }
    }
    public class rto_ob_report_list
    {
        public long RTOVOB_Id { get; set; }
        public long VMSFOVOB_Id { get; set; }

        public string VMSVKEYS_Id { get; set; }
    }
}