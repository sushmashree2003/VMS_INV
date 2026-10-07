using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class SMSMasterApprovalDTO
    {
        public long SMA_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int SMA_Level { get; set; }
        public long IVRMSTAUL_Id { get; set; }
        public string SMA_SMSMailCallFlag { get; set; }
        public bool SMA_ActiveFlag { get; set; }
        public string IVRMSTAUL_UserName { get; set; }
        public Array userNamelist { get; set; }
        public bool returnval { get; set; }
        public Array smsemailaplist { get; set; }
        public Array editdata { get; set; }
        public string dup { get; set; }
        public string headername { get; set; }
        public Array headernamelist { get; set; }

    }
}
