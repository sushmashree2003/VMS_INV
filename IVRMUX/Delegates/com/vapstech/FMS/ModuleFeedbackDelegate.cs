using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FMS;

namespace IVRMUX.Delegates.com.vapstech.FMS
{
    public class ModuleFeedbackDelegate
    {
        CommonDelegate<ModuleFeedbackDTO, ModuleFeedbackDTO> _comm = new CommonDelegate<ModuleFeedbackDTO, ModuleFeedbackDTO>();

        public ModuleFeedbackDTO getfeedback(ModuleFeedbackDTO data)
        {
            return _comm.FMSGetPostData(data, "ModuleFeedbackFacade/getfeedback");
        }
        public ModuleFeedbackDTO submitfeedback(ModuleFeedbackDTO data)
        {
            return _comm.FMSGetPostData(data, "ModuleFeedbackFacade/submitfeedback");
        }
        public ModuleFeedbackDTO checkfeedback(ModuleFeedbackDTO data)
        {
            return _comm.FMSGetPostData(data, "ModuleFeedbackFacade/checkfeedback");
        }
    }
}
