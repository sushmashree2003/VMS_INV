using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.RTO
{
    public class VMS_Vehicle_ReportDTO
    {
       
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array vehicales { get; set; }
        public Array GetReport { get; set; }
        public Array get_report { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Vehicalist[] Vehicalist { get; set; }
        public string milogo { get; set; }
    }
    public class Vehicalist
    {
        public long VMSVKEYS_Id { get; set; }
    }
}
