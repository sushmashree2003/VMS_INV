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
    public class ISM_PrioritySwitchReportFacade : Controller
    {

        public ISM_PrioritySwitchReportInterface _objinter;

        public ISM_PrioritySwitchReportFacade(ISM_PrioritySwitchReportInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_PrioritySwitchReportDTO> getdetails([FromBody] ISM_PrioritySwitchReportDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getdeviationReport")]
        public Task<ISM_PrioritySwitchReportDTO> getdeviationReport([FromBody]ISM_PrioritySwitchReportDTO data)
        {
            return _objinter.getdeviationReport(data);
        }

        //============================= weekly deviation
        [Route("getdetails_weekly")]
        public Task<ISM_PrioritySwitchReportDTO> getdetails_weekly([FromBody] ISM_PrioritySwitchReportDTO data)
        {
            return _objinter.getdetails_weekly(data);
        }

        [Route("getdeviationReport_weekly")]
        public Task<ISM_PrioritySwitchReportDTO> getdeviationReport_weekly([FromBody]ISM_PrioritySwitchReportDTO data)
        {
            return _objinter.getdeviationReport_weekly(data);
        }

       
    }
}
