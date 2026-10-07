using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.RTO
{
   public class RTO_Master_FineDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public string returnval { get; set; }
        public Array RTOArray { get; set; }
        public long RTOMFP_Id { get; set; }
        public string RTOMFP_FineParameterName { get; set; }
        public decimal RTOMFP_FineAmount { get; set; }
        public long RTOMFP_MaxViolationPermitted { get; set; }
        public string RTOMFP_ParameterDescription { get; set; }
        public bool RTOMFP_ActiveFlag { get; set; }
        public Array emplist { get; set; }
        public Array vehicalArray { get; set; }
        public Array vehicalList { get; set; }
        public Array Vehicales { get; set; }
        public long TRMV_Id { get; set; }
    }
}
