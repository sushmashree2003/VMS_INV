using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_DailyReportGenerationController : Controller
    {
        ISM_DailyReportGeneratioDelegate _delg = new ISM_DailyReportGeneratioDelegate();

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

        [Route("loaddata/{id:int}")]
        public ISM_DailyReportGenerationDTO loaddata(int id)
        {
            ISM_DailyReportGenerationDTO data = new ISM_DailyReportGenerationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.DR_Notification = Convert.ToInt64(HttpContext.Session.GetInt32("DR_Notification"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.loaddata(data);
        }

        [Route("generatedailyreport")]
        public ISM_DailyReportGenerationDTO generatedailyreport([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.generatedailyreport(data);
        }

        [Route("searchpreviousdailyreport")]
        public ISM_DailyReportGenerationDTO searchpreviousdailyreport([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.searchpreviousdailyreport(data);
        }

        [Route("addextratask")]
        public ISM_DailyReportGenerationDTO addextratask([FromBody] ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.addextratask(data);
        }

        [Route("viewresponse")]
        public ISM_DailyReportGenerationDTO viewresponse([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.viewresponse(data);
        }

        [Route("Otherdaycount")]
        public ISM_DailyReportGenerationDTO Otherdaycount([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.Otherdaycount(data);
        }

        [Route("SaveRemarks")]
        public ISM_DailyReportGenerationDTO SaveRemarks([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            HttpContext.Session.SetInt32("DR_Notification", 1);
            return _delg.SaveRemarks(data);
        }

        [Route("SaveDRNotSentRemarks")]
        public ISM_DailyReportGenerationDTO SaveDRNotSentRemarks([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            HttpContext.Session.SetInt32("DR_Notification", 1);
            return _delg.SaveDRNotSentRemarks(data);
        }

        [Route("CheckDeviationRemarks")]
        public ISM_DailyReportGenerationDTO CheckDeviationRemarks([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            HttpContext.Session.SetInt32("DR_Notification", 1);
            return _delg.CheckDeviationRemarks(data);
        }

        [Route("ViewCheckListSavedDetails")]
        public ISM_DailyReportGenerationDTO ViewCheckListSavedDetails([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            HttpContext.Session.SetInt32("DR_Notification", 1);
            return _delg.ViewCheckListSavedDetails(data);
        }

        [Route("ViewBlockedDetails")]
        public ISM_DailyReportGenerationDTO ViewBlockedDetails([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));            
            return _delg.ViewBlockedDetails(data);
        }

        [Route("GetCategoryChecklistDetails")]
        public ISM_DailyReportGenerationDTO GetCategoryChecklistDetails([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));            
            return _delg.GetCategoryChecklistDetails(data);
        }

        [Route("ViewDRNotApprovedList")]
        public ISM_DailyReportGenerationDTO ViewDRNotApprovedList([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));            
            return _delg.ViewDRNotApprovedList(data);
        }

        [Route("DRGenerationreject")]
        public ISM_DailyReportGenerationDTO DRGenerationreject([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DRGenerationreject(data);
        }
        [Route("generateRejectedtask")]
        public ISM_DailyReportGenerationDTO generateRejectedtask([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.generateRejectedtask(data);
        }
        [Route("saveDRremarks")]
        public ISM_DailyReportGenerationDTO saveDRremarks([FromBody]  ISM_DailyReportGenerationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _delg.saveDRremarks(data);
        }
    }
}
