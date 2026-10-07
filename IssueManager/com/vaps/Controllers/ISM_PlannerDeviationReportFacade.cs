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
    public class ISM_PlannerDeviationReportFacade : Controller
    {

        public ISM_PlannerDeviationReportInterface _objinter;

        public ISM_PlannerDeviationReportFacade(ISM_PlannerDeviationReportInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_PlannerReportsDTO> getdetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getdeviationReport")]
        public Task<ISM_PlannerReportsDTO> getdeviationReport([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.getdeviationReport(data);
        }

        //============================= weekly deviation
        [Route("getdetails_weekly")]
        public Task<ISM_PlannerReportsDTO> getdetails_weekly([FromBody] ISM_PlannerReportsDTO data)
        {
            return _objinter.getdetails_weekly(data);
        }

        [Route("getdeviationReport_weekly")]
        public Task<ISM_PlannerReportsDTO> getdeviationReport_weekly([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.getdeviationReport_weekly(data);
        }

       
    }
}
