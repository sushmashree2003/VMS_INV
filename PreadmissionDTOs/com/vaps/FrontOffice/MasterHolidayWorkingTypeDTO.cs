using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class MasterHolidayWorkingTypeDTO
    {
        public long MI_Id { get; set; }

        public long userid { get; set; }
        public long FOHWDT_Id { get; set; }
        public Array getarray { get; set; }

        public String FOHTWD_HolidayWDType { get; set; }
        public string FOHTWD_HolidayWDTypeFlag { get; set; }
        public bool FOHTWD_HolidayFlag { get; set; }

        public string returnmsg { get; set; }

    }
}
