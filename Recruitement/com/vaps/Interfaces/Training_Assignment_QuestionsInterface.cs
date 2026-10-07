using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
   public interface Training_Assignment_QuestionsInterface
    {
        HR_Training_Assignment_Questions_DTO getdata_question(HR_Training_Assignment_Questions_DTO dto);

        HR_Training_Assignment_Questions_DTO SaveEdit_question(HR_Training_Assignment_Questions_DTO dto);

        HR_Training_Assignment_Questions_DTO details_question(HR_Training_Assignment_Questions_DTO id);

        HR_Training_Assignment_Questions_DTO deactivate_question(HR_Training_Assignment_Questions_DTO id);
    }
}
