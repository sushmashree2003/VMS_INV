using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Assignment_Questions_DTO
    {
        public long HRTAQNS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMTT_Id { get; set; }
        public string HRTAQNS_QuestionName { get; set; }
        public string HRTAQNS_QuestionTypeFlg { get; set; }
        public long HRTAQNS_QuestionOrder { get; set; }
        public bool HRTAQNS_ActiveFlg { get; set; }
        public long userId { get; set; }
        public string returnvalue { get; set; }
        public Array question_list { get; set; }

        public Array question_details_list { get; set; }

        public bool returnval { get; set; }

    }
    
}
