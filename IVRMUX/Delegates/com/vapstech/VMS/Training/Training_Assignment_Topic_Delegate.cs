using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Training_Assignment_Topic_Delegate
    {
        CommonVMSDelegate<HR_Training_Assignment_Topic_DTO, HR_Training_Assignment_Topic_DTO> COMMTAQO = new CommonVMSDelegate<HR_Training_Assignment_Topic_DTO, HR_Training_Assignment_Topic_DTO>();

        public HR_Training_Assignment_Topic_DTO getdata_TAT(HR_Training_Assignment_Topic_DTO dto)
        {
            return COMMTAQO.POSTData(dto, "Training_Assignment_TopicFacade/getdata_TAT/");
        }
    }
}
