using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Legal
{
    public class LegalNoticeDTO
    {
        public long MI_Id { get; set; }
        public long FLAG { get; set; }
        public long ASMAY_Id { get; set; }
        public long roleId { get; set; }
        public Array employees { get; set; }
        public Array clientlist { get; set; }
        public Array vendorlist { get; set; }
        public Array FlagOnLoadOrOnChangeInst { get; set; }
    }
}
