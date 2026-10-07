using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FMS
{
    public class Employee_Wise_VMS_FeedBackDTO
    {
        public long MI_Id { set; get; }
        public long? userid { get; set; }
        public Array employee_list { get; set; }
        public Array Feedbackdata { get; set; }
        public string saved { get; set; }
        public Feedback_listDTO[] Feedback_list{get;set;}
    }
    public class Feedback_listDTO
    {
        public long HRME_Id { get; set; }
        public string VEF_Remarks { get; set; }
        public bool VEF_NegativeFlag { get; set; }
    }
}
