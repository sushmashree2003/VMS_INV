using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
   public class HR_Training_Feedback_DTO
    {
        public long HRTFEED_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMTT_Id { get; set; }

        public long HRTCR_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRTFEED_IntORExtFlg { get; set; }
        public string HRMTT_Topic { get; set; }
        public long HRTFEED_TrainerId { get; set; }
        public long HRMFQNS_Id { get; set; }
        public long HRMFOPT_Id { get; set; }
        public string HRTFEED_AboutTraining { get; set; }
        public string HRTFEED_Improvement { get; set; }
        public string HRTFEED_Response { get; set; }
        public bool HRTFEED_ActiveFlg { get; set; }
        public long HRTFEED_CreatedBy { get; set; }
        public long HRTFEED_UpdatedBy { get; set; }
        public DateTime HRTFEED_CreatedDate { get; set; }
        public DateTime HRTFEED_UpdatedDate { get; set; }
        public long userId { get; set; }
        public string HRTCR_PrgogramName { get; set; }
        public string HRTCR_ProgramDesc { get; set; }
        public string Type { get; set; }
        public string HRMFQNS_QuestionName { get; set; }
        public string HRMFQNS_QuestionTypeFlg { get; set; }
        public string HRMFOPT_OptionName { get; set; }
        public string returnvales { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long answer { get; set; }
        public bool? HRTFEED_Verified { get; set; }
        public Array traninglisttrainer { get; set; }
        public Array traninglisttrainee { get; set; }
        public Array mappedquestionlist { get; set; }
        public Array getmasterdata { get; set; }
        public Array mappedoptionlist { get; set; }
        public Array trainingFeedbacklist { get; set; }
        public Array trainerlist { get; set; }
        public Array traineelist { get; set; }
        public Array getfeedbackreport { get; set; }
        public HR_Training_Feedback_DTO[] question_Answer { get; set; }
        public string name { get; set; }
        public Array totalparticipants { get; set; }
        public Array getfeedbackquestions { get; set; }
        public long HRMLY_Id { get; set; }
        public string feedbacktype { get; set; }
        public string Monthid { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array trainingfeedbackdetails { get; set; }
        public Array traineeprogdetails { get; set; }
        public Array trainerprogdetails { get; set; }
    }
}
