using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMS.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FMS;

namespace FMS.Com.Facade
{
    [Route("api/[controller]")]
    public class ModuleFeedbackFacade :Controller
    {
        public ModuleFeedbackInterface _interface;
        public ModuleFeedbackFacade(ModuleFeedbackInterface _inter)
        {
            _interface = _inter;
        }

        [Route("getfeedback")]
        public ModuleFeedbackDTO getfeedback([FromBody] ModuleFeedbackDTO data)
        {
            return _interface.getfeedback(data);
        }
        [Route("submitfeedback")]
        public ModuleFeedbackDTO submitfeedback([FromBody] ModuleFeedbackDTO data)
        {
            return _interface.submitfeedback(data);
        }
        [Route("checkfeedback")]
        public ModuleFeedbackDTO checkfeedback([FromBody] ModuleFeedbackDTO data)
        {
            return _interface.checkfeedback(data);
        }
    }
}
