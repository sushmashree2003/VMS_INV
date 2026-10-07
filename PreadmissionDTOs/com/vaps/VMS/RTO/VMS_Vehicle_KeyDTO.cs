using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.RTO
{
    public class VMS_Vehicle_KeyDTO
    {
        public long UserId { get; set; }
        public string returnval { get; set; }
        public long VMSVKEYS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TRMV_Id { get; set; }
        public DateTime? VMSVKEYS_DateTime { get; set; }
        public string VMSVKEYS_KeyReceivedHandedOverFlg { get; set; }
        public bool VMSVKEYS_ActiveFlag { get; set; }
        public string VMSVKEYS_Time { get; set; }
        public string VMSVKEYS_RecivedTime { get; set; }
        public DateTime? VMSVKEYS_RecivedDate { get; set; }
        public string VMSVKEYS_Remarks { get; set; }
        public string VMSVKEYS_KeyHandedOverFlg { get; set; }
        public long? VMSVKEYS_OdometerReading { get; set; }
    }
}
