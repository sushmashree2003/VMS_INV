using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_InteractionFacade : Controller
    {

        public ISM_InteractionInterface _objinter;

        public ISM_InteractionFacade(ISM_InteractionInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_InteractionDTO> getdetails([FromBody] ISM_InteractionDTO data)
        {
            return _objinter.getdetails(data);
        }
        
        [Route("savedata")]
        public ISM_InteractionDTO savedata([FromBody]ISM_InteractionDTO data)
        {
            return _objinter.savedata(data);
        }
        [Route("get_replydetails")]
        public Task<ISM_InteractionDTO> get_replydetails([FromBody] ISM_InteractionDTO data)
        {
            return _objinter.get_replydetails(data);
        }
        [Route("savereply")]
        public ISM_InteractionDTO savereply([FromBody]ISM_InteractionDTO data)
        {
            return _objinter.savereply(data);
        }

        //=======================feedback====================
         [Route("getfeedbac")]
        public ISM_InteractionDTO getfeedbac([FromBody]ISM_InteractionDTO data)
        {
            return _objinter.getfeedbac(data);
        }
         [Route("feedback_msg_details")]
        public ISM_InteractionDTO feedback_msg_details([FromBody]ISM_InteractionDTO data)
        {
            return _objinter.feedback_msg_details(data);
        }
        [Route("desktopnotification")]
        public async Task<ISM_InteractionDTO> desktopnotification([FromBody]ISM_InteractionDTO data)
        {
            return await _objinter.desktopnotification(data);
        }
        [Route("Sendfeedback_tfr")]
        public ISM_InteractionDTO Sendfeedback_tfr([FromBody]ISM_InteractionDTO data)
        {
            return _objinter.Sendfeedback_tfr(data);
        }
         [Route("getViewfeedback")]
        public ISM_InteractionDTO getViewfeedback([FromBody]ISM_InteractionDTO data)
        {
            return _objinter.getViewfeedback(data);
        }

        //=======================feedback====================
        [Route("getnotification")]
        public ISM_InteractionDTO getnotification([FromBody]ISM_InteractionDTO data)
        {
            return _objinter.getnotification(data);
        }
        [Route("clicknotification")]
        public ISM_InteractionDTO clicknotification([FromBody]ISM_InteractionDTO data)
        {
            return _objinter.clicknotification(data);
        }
    }
}
