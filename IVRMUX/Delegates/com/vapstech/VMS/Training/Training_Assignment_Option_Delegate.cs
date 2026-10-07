using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Training_Assignment_Option_Delegate
    {
        CommonVMSDelegate<HR_Training_Assignment_Option_DTO, HR_Training_Assignment_Option_DTO> COMMMFO = new CommonVMSDelegate<HR_Training_Assignment_Option_DTO, HR_Training_Assignment_Option_DTO>();

        public HR_Training_Assignment_Option_DTO getdata_MFO(HR_Training_Assignment_Option_DTO dto)
        {
            return COMMMFO.POSTData(dto, "Training_Assignment_OptionFacade/getdata_MFO/");
        }


        public HR_Training_Assignment_Option_DTO SaveEdit_MFO(HR_Training_Assignment_Option_DTO dto)
        {
            return COMMMFO.POSTData(dto, "Training_Assignment_OptionFacade/SaveEdit_MFO/");
        }
        public HR_Training_Assignment_Option_DTO deactivate_MFO(HR_Training_Assignment_Option_DTO dto)
        {
            return COMMMFO.POSTData(dto, "Training_Assignment_OptionFacade/deactivate_MFO/");
        }
        public HR_Training_Assignment_Option_DTO details_MFO(HR_Training_Assignment_Option_DTO id)
        {
            return COMMMFO.POSTData(id, "Training_Assignment_OptionFacade/details_MFO/");
        }


    }
}
