using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Training_Feedback_Interface
    {
        //---------------------- building-----------------------
        HR_Training_Feedback_DTO loaddata(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO getQuestions(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO getFeedbackTrainer(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO savetrainerfeedback(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO savetraineefeedback(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO trainerfeedbackreport(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO traineefeedbackreport(HR_Training_Feedback_DTO dto);

        ///report////////////////////////////////////////////////
        HR_Training_Feedback_DTO reportloaddata(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO reportpagetrainerfeedbackreport(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO reportpagetraineefeedbackreport(HR_Training_Feedback_DTO dto);
        HR_Training_Feedback_DTO trainigheldreport(HR_Training_Feedback_DTO dto);
    }
}