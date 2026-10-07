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
    public class ISM_ConsolidateDailyReportFacade : Controller
    {

        public ISM_ConsolidateDailyReportInterface _objinter;

        public ISM_ConsolidateDailyReportFacade(ISM_ConsolidateDailyReportInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_DailyReportGenerationDTO> getdetails([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getReport")]
        public Task<ISM_DailyReportGenerationDTO> getReport([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _objinter.getReport(data);
        }

        [Route("ViewCheckListSavedDetails")]
        public ISM_DailyReportGenerationDTO ViewCheckListSavedDetails([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _objinter.ViewCheckListSavedDetails(data);
        }

        [Route("viewresponse")]
        public ISM_DailyReportGenerationDTO viewresponse([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _objinter.viewresponse(data);
        }

        [Route("getfeedbackReport")]
        public Task<ISM_DailyReportGenerationDTO> getfeedbackReport([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _objinter.getfeedbackReport(data);
        }

        [Route("viewcomments")]
        public Task<ISM_DailyReportGenerationDTO> viewcomments([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _objinter.viewcomments(data);
        }

        [Route("getmodulelist")]
        public ISM_DailyReportGenerationDTO getmodulelist([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _objinter.getmodulelist(data);
        }

        [Route("getcomplinetstaskreport")]
        public ISM_DailyReportGenerationDTO getcomplinetstaskreport([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _objinter.getcomplinetstaskreport(data);
        }

        [Route("getyerlyplannerreport")]
        public ISM_DailyReportGenerationDTO getyerlyplannerreport([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _objinter.getyerlyplannerreport(data);
        }
    }
}
