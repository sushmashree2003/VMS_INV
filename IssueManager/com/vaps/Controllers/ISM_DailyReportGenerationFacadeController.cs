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
    public class ISM_DailyReportGenerationFacadeController : Controller
    {
        public ISM_DailyReportGenerationInterface _interface;

        public ISM_DailyReportGenerationFacadeController(ISM_DailyReportGenerationInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("loaddata")]
        public ISM_DailyReportGenerationDTO loaddata([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.loaddata(data);
        }

        [Route("generatedailyreport")]
        public ISM_DailyReportGenerationDTO generatedailyreport([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.generatedailyreport(data);
        }

        [Route("searchpreviousdailyreport")]
        public ISM_DailyReportGenerationDTO searchpreviousdailyreport([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.searchpreviousdailyreport(data);
        }

        [Route("addextratask")]
        public ISM_DailyReportGenerationDTO addextratask([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.addextratask(data);
        }

        [Route("viewresponse")]
        public ISM_DailyReportGenerationDTO viewresponse([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.viewresponse(data);
        }

        [Route("Otherdaycount")]
        public ISM_DailyReportGenerationDTO Otherdaycount([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.Otherdaycount(data);
        }

        [Route("SaveRemarks")]
        public ISM_DailyReportGenerationDTO SaveRemarks([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.SaveRemarks(data);
        }

        [Route("SaveDRNotSentRemarks")]
        public ISM_DailyReportGenerationDTO SaveDRNotSentRemarks([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.SaveDRNotSentRemarks(data);
        }

        [Route("CheckDeviationRemarks")]
        public ISM_DailyReportGenerationDTO CheckDeviationRemarks([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.CheckDeviationRemarks(data);
        }

        [Route("ViewCheckListSavedDetails")]
        public ISM_DailyReportGenerationDTO ViewCheckListSavedDetails([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.ViewCheckListSavedDetails(data);
        }

        [Route("ViewBlockedDetails")]
        public ISM_DailyReportGenerationDTO ViewBlockedDetails([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.ViewBlockedDetails(data);
        }

        [Route("GetCategoryChecklistDetails")]
        public ISM_DailyReportGenerationDTO GetCategoryChecklistDetails([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.GetCategoryChecklistDetails(data);
        }

        [Route("ViewDRNotApprovedList")]
        public ISM_DailyReportGenerationDTO ViewDRNotApprovedList([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.ViewDRNotApprovedList(data);
        }

        [HttpPost]
        [Route("TriggerPlannerMail")]
        public ISM_DailyReportGenerationDTO TriggerPlannerMail([FromBody]ISM_DailyReportGenerationDTO data)
        {
            return _interface.TriggerPlannerMail(data);
        }
        [Route("DRGenerationreject")]
        public ISM_DailyReportGenerationDTO DRGenerationreject([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.DRGenerationreject(data);
        }
        [Route("generateRejectedtask")]
        public ISM_DailyReportGenerationDTO generateRejectedtask([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.generateRejectedtask(data);
        }
        [Route("saveDRremarks")]
        public ISM_DailyReportGenerationDTO saveDRremarks([FromBody] ISM_DailyReportGenerationDTO data)
        {
            return _interface.saveDRremarks(data);
        }

    }
}