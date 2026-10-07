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
    public class ISM_NotificationFacade : Controller
    {

        public ISM_NotificationInterface _objinter;

        public ISM_NotificationFacade(ISM_NotificationInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_NotificationDTO> getdetails([FromBody] ISM_NotificationDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getnotificationdetails")]
        public Task<ISM_NotificationDTO> getnotificationdetails([FromBody]ISM_NotificationDTO data)
        {
            return _objinter.getnotificationdetails(data);
        }
        [Route("pushnotificationdetails")]
        public Task<ISM_NotificationDTO> pushnotificationdetails([FromBody]ISM_NotificationDTO data)
        {
            return _objinter.pushnotificationdetails(data);
        }

        [Route("getplannernotgenerated")]
        public Task<ISM_NotificationDTO> getplannernotgenerated([FromBody]ISM_NotificationDTO data)
        {
            return _objinter.getplannernotgenerated(data);
        }
        [Route("emialnotificationfordailyreportblock")]
        public Task<ISM_NotificationDTO> emialnotificationfordailyreportblock([FromBody]ISM_NotificationDTO data)
        {
            return _objinter.emialnotificationfordailyreportblock(data);
        }
        [Route("getplannernotapproved")]
        public Task<ISM_NotificationDTO> getplannernotapproved([FromBody]ISM_NotificationDTO data)
        {
            return _objinter.getplannernotapproved(data);
        }
        [Route("getplannernotapprovedmd")]
        public Task<ISM_NotificationDTO> getplannernotapprovedmd([FromBody]ISM_NotificationDTO data)
        {
            return _objinter.getplannernotapprovedmd(data);
        }
        [Route("getdailyreportnotapproved")]
        public Task<ISM_NotificationDTO> getdailyreportnotapproved([FromBody]ISM_NotificationDTO data)
        {
            return _objinter.getdailyreportnotapproved(data);
        }
        [Route("unauthorizednotice")]
        public Task<ISM_NotificationDTO> unauthorizednotice([FromBody] ISM_NotificationDTO data)
        {
            return _objinter.unauthorizednotice(data);
        }
        [Route("lateinearlyoutnotice")]
        public Task<ISM_NotificationDTO> lateinearlyoutnotice([FromBody] ISM_NotificationDTO data)
        {
            return _objinter.lateinearlyoutnotice(data);
        }
        [Route("noticeProbition")]
        public Task<ISM_NotificationDTO> noticeProbition([FromBody] ISM_NotificationDTO data)
        {
            return _objinter.noticeProbition(data);
        }
    }
}
