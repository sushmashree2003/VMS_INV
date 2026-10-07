using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.RTO
{
  public  class FO_Vehicle_Ob_reportDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public Array fovehicales { get; set; }
        public Array GetReportt { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
       public fo_ob_report_list[] fo_ob_report_list { get; set; }
    }
   public class fo_ob_report_list
   {
        public long VMSFOVOB_Id { get; set; }
        public string VMSVKEYS_Id { get; set; }
    }
}
