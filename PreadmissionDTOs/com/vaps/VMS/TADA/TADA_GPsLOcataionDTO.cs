using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.TADA
{
   public class TADA_GPsLOcataionDTO
    {
        public long MI_Id { get; set; }
        public long User_Id { get; set; }
        public long HRME_Id { get; set; }
        public Array getReport { get; set; }
        public Array alldata { get; set; }
        public DateTime? Fromdate { get; set; }
        public DateTime? Todate { get; set; }
        public TADA_GPsLOcataion[] TADA_GPsLOcataion { get; set; }
        public DateTime? VMSGPSTADA_DateTime { get; set; }
        public string  VMSGPSTADA_Address { get; set; }
        public string VMSGPSTADA_LATLONGValue { get; set; }
        public string VMSGPSTADA_Remarks { get; set; }
        public bool VMSGPSTADA_ActiveFlg { get; set; }
        public string message { get; set; }
        public long ISMSLE_Id { get; set; }
        public long IER_Id { get; set; }

    }
    public class TADA_GPsLOcataion
    {
        public long HRME_Id { get; set; }
        public string From_date { get; set; }
        public string To_date { get; set; }

    }
  //  public TADA_GPsLOcataion


}
