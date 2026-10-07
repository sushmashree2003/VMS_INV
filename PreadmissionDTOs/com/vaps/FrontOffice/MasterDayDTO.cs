using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class MasterDayDTO
    {
        public long MI_Id { get; set; }
        public long FOMD_Id { get; set; }
        public long userid { get; set; }
        public Array getarray { get; set; }
        public bool FOMD_ActiveFlag { get; set; }

        public string returnduplicatestatus { get; set; }

        public string FOMD_DayName { get; set; }
        public string FOMD_DayCode { get; set; }
        public string returnmsg { get; set; }

        public bool returnval { get; set; }

     //   public string FOMD_DayCode { get; set; }
    }
}
