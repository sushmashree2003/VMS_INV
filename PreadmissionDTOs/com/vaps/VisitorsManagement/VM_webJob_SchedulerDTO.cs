using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class VM_webJob_SchedulerDTO
    {
        public long MI_Id { get; set; }
        public string type { get; set; }
        public string typeflag { get; set; }
        public string result { get; set; }
        public string visitorname { get; set; }
        public long visitorid { get; set; }
        public string towhommet { get; set; }
        public string datetime { get; set; }
        public string time { get; set; }
        public string emailid { get; set; }
        public long mobileno { get; set; }
    }
}
