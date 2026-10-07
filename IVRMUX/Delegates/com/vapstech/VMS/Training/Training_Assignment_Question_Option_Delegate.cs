using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Training_Assignment_Question_Option_Delegate
    {
        CommonVMSDelegate<HR_Training_Assignment_Question_Option_DTO, HR_Training_Assignment_Question_Option_DTO> COMMTAQO = new CommonVMSDelegate<HR_Training_Assignment_Question_Option_DTO, HR_Training_Assignment_Question_Option_DTO>();

        public HR_Training_Assignment_Question_Option_DTO getdata_TAQO(HR_Training_Assignment_Question_Option_DTO dto)
        {
            return COMMTAQO.POSTData(dto, "Training_Assignment_Question_OptionFacade/getdata_TAQO/");
        }
        public HR_Training_Assignment_Question_Option_DTO SaveEdit_TAQO(HR_Training_Assignment_Question_Option_DTO dto)
        {
            return COMMTAQO.POSTData(dto, "Training_Assignment_Question_OptionFacade/SaveEdit_TAQO/");
        }
        public HR_Training_Assignment_Question_Option_DTO deactivate_TAQO(HR_Training_Assignment_Question_Option_DTO dto)
        {
            return COMMTAQO.POSTData(dto, "Training_Assignment_Question_OptionFacade/deactivate_TAQO/");
        }
        public HR_Training_Assignment_Question_Option_DTO details_TAQO(HR_Training_Assignment_Question_Option_DTO id)
        {
            return COMMTAQO.POSTData(id, "Training_Assignment_Question_OptionFacade/details_TAQO/");
        }
        public HR_Training_Assignment_Question_Option_DTO option_view_TAQO(HR_Training_Assignment_Question_Option_DTO id)
        {
            return COMMTAQO.POSTData(id, "Training_Assignment_Question_OptionFacade/option_view_TAQO/");
        }
    }
}
