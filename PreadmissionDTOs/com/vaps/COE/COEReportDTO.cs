using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PreadmissionDTOs.com.vaps.COE
{
    public class COEReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string type { get; set; }
        public int month { get; set; }

        public Array yearlist { get; set; }
     
        public Array classlist { get; set; }
        public Array coereport { get; set; }
        public int count { get; set; }
        public string eventName { get; set; }
        public string eventDesc { get; set; }
        public  DateTime? COEE_EStartDate { get; set; }
        public DateTime? COEE_EEndDate { get; set; }
        public Array fillmonth { get; set; }
        public Array fillyear { get; set; }
        public long year { get; set; }
        public Array ClassList { get; set; }
        public Array institute { get; set; }

        //Sudarshan
       
        public Array modulenames { get; set; }
        public module_array[] modulearray { get; set; }

        public class module_array
        {
            public long IVRMM_Id { get; set; }
            public string IVRMM_ModuleName { get; set; }
        }

    }
}
