using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.FMS;

namespace FMS.Com.Interface
{
   public interface ModuleFeedbackInterface 
    {
        ModuleFeedbackDTO getfeedback(ModuleFeedbackDTO data);
        ModuleFeedbackDTO submitfeedback(ModuleFeedbackDTO data);
        ModuleFeedbackDTO checkfeedback(ModuleFeedbackDTO data);
    }
}
