using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Training_Assignment_Option_DTO
    {

     
        public long MI_Id { get; set; }
        public long userId { get; set; }
        public long HRTAOPT_Id { get; set; }
        public string HRTAOPT_OptionName { get; set; }
        public long HRTAOPT_OptionOrder { get; set; }
        public string returnvalue { get; set; }
        public bool HRTAOPT_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public Array question_list { get; set; }
        public long? HRTAOPT_UpdatedBy { get; set; }
        public DateTime? HRTAOPT_UpdatedDate { get; set; }
        public long? HRTAOPT_CreatedBy { get; set; }
        public DateTime? HRTAOPT_CreatedDate { get; set; }
        

        public Array question_details_list { get; set; }

        






    }
}
