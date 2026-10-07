using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Training_Assignment_Questions_Delegate
    {
        CommonVMSDelegate<HR_Training_Assignment_Questions_DTO, HR_Training_Assignment_Questions_DTO> COMMquestion = new CommonVMSDelegate<HR_Training_Assignment_Questions_DTO, HR_Training_Assignment_Questions_DTO>();
        public HR_Training_Assignment_Questions_DTO getdata_question(HR_Training_Assignment_Questions_DTO dto)
        {
            return COMMquestion.POSTData(dto, "Training_Assignment_QuestionsFacade/getdata_question/");
        }

        public HR_Training_Assignment_Questions_DTO SaveEdit_question(HR_Training_Assignment_Questions_DTO dto)
        {
            return COMMquestion.POSTData(dto, "Training_Assignment_QuestionsFacade/SaveEdit_question/");
        }


        public HR_Training_Assignment_Questions_DTO details_question(HR_Training_Assignment_Questions_DTO id)
        {
            return COMMquestion.POSTData(id, "Training_Assignment_QuestionsFacade/details_question/");
        }

        public HR_Training_Assignment_Questions_DTO deactivate_question(HR_Training_Assignment_Questions_DTO dto)
        {
            return COMMquestion.POSTData(dto, "Training_Assignment_QuestionsFacade/deactivate_question/");
        }
    }
}
