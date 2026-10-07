using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.IssueManager
{
   public class workingdays_DTO:CommonParamDTO
    {
        public long ISMMWD_Id { get; set; }
        public long MI_Id { get; set; }
        public long FOMD_Id { get; set; }
        //   public long ISMMWD_Hours { get; set; }
        public decimal? ISMMWD_Hours { get; set; }
        public bool ISMPRTY_ActiveFlag { get; set; }
        public long ISMMWD_CreatedBy { get; set; }
        public long ISMMWD_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public string FOMD_DayName { get; set; }

        public Array daylist { get; set; }
        public Array alldata { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public workingdays_DTO[] daylisdata { get; set; }

    }
}
