using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TeleCaller
{
   public class TeleCallerDTO
    {
     public string ClientName { get; set; }
        public string Address { get; set; }
        public string TotalStrength { get; set; }
        public string Product { get; set; }
        public string Status { get; set; }
        public int MI_Id { get; set; }
        public int ASMAY_ID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? Todate { get; set; }
        public string Flag { get; set; }
        public string RemarkExeFlag { get; set; }
        public string RemarksFlag { get; set; }
        
        public string Remarks { get; set; }
        public long userid { get; set; }

        public string ClientNumber { get; set; }
        public string contactPerson { get; set; }
        public Array TelecallerList { get; set; }
        public Array Updateclientlist { get; set; }
        public Array PresentationClientlist { get; set; }
        public DateTime? Demodate { get; set; }
        public int ISMSLE_Id { get; set; }
        public int ISMSLEDM_Id { get; set; }
        public int ISMSMPR_Id { get; set; }
        public string CER_Remarks { get; set; }
        public string TelecallerRemarks { get; set; }
        
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdateBy { get; set; }

        public string UpdateOn { get; set; }

        public string message { get; set; }

        public string message1 { get; set; }
    }

}
