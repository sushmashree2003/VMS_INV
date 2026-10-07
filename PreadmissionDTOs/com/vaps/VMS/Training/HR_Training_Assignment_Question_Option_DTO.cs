using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Assignment_Question_Option_DTO
    {
        public long MI_Id { get; set; }
        public Array question_option_list { get; set; }
        public string HRTAOPT_OptionName { get; set; }
        public Array option_list { get; set; }
        public long HRTAQNS_Id { get; set; }
        public long HRTAOPT_Id { get; set; }
        public string HRTAQNS_QuestionName { get; set; }
        public bool HRTAQOPT_ActiveFlg { get; set; }
        public Array question_list { get; set; }
        public bool HRTAOPT_ActiveFlg { get; set; }
        public long userId { get; set; }
        public long hqn_id { get; set; }
        public bool returnval { get; set; }
        public string returnvalue { get; set; }
        public Array question_details_list { get; set; }
        public Array option_details_list { get; set; }
        public Array option_view_list { get; set; }
        public HR_Training_Assignment_Question_Option_New[] Feedback_Option { get; set; }


        public class HR_Training_Assignment_Question_Option_New
        {
            public long HRTAOPT_Id { get; set; }
        }



    }
}
