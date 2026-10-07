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
    public class ISM_NoticeIssueFacade : Controller
    {

        public ISM_NoticeIssueInterface _objinter;

        public ISM_NoticeIssueFacade(ISM_NoticeIssueInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_PlannerCreationDTO> getdetails([FromBody] ISM_PlannerCreationDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getplannerlist")]
        public Task<ISM_PlannerCreationDTO> getplannerlist([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.getplannerlist(data);
        }      
    
        [Route("getplannerdetails")]
        public Task<ISM_PlannerCreationDTO> getplannerdetails([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.getplannerdetails(data);
        }
        [Route("updateplannerStatus")]
        public ISM_PlannerCreationDTO updateplannerStatus([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.updateplannerStatus(data);
        }
        [Route("allupdateplannerStatus")]
        public ISM_PlannerCreationDTO allupdateplannerStatus([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.allupdateplannerStatus(data);
        }
        [Route("saveAddTask")]
        public ISM_PlannerCreationDTO saveAddTask([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.saveAddTask(data);
        }
        [Route("getextratask")]
        public Task<ISM_PlannerCreationDTO> getextratask([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.getextratask(data);
        }
        
        [Route("getApprovedPlannerDetail")]
        public Task<ISM_PlannerCreationDTO> getApprovedPlannerDetail([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.getApprovedPlannerDetail(data);
        }

        [Route("sendschedularnotice")]
        public Task<ISM_PlannerReportsDTO> sendschedularnotice([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.sendschedularnotice(data);
        }

        [Route("sendschedularplannernotice")]
        public Task<ISM_PlannerReportsDTO> sendschedularplannernotice([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objinter.sendschedularplannernotice(data);
        }




    }
}
