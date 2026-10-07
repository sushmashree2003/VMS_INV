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
    public class ISM_PlannerCreationFacade : Controller
    {

        public ISM_PlannerCreationInterface _objinter;

        public ISM_PlannerCreationFacade(ISM_PlannerCreationInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_PlannerCreationDTO> getdetails([FromBody] ISM_PlannerCreationDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("gettaskdetails")]
        public Task<ISM_PlannerCreationDTO> gettaskdetails([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.gettaskdetails(data);
        }

        [Route("geteffortdetails")]
        public Task<ISM_PlannerCreationDTO> geteffortdetails([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.geteffortdetails(data);
        }
        [Route("savePlanner")]
        public ISM_PlannerCreationDTO savePlanner([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.savePlanner(data);
        }
        [Route("saveAddTask")]
        public ISM_PlannerCreationDTO saveAddTask([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.saveAddTask(data);
        }

        [Route("removetask")]
        public ISM_PlannerCreationDTO removetask([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.removetask(data);
        }

        [Route("getplannerdetails")]
        public Task<ISM_PlannerCreationDTO> getplannerdetails([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.getplannerdetails(data);
        }

        [Route("TriggerPlannerMail")]
        public ISM_PlannerCreationDTO TriggerPlannerMail([FromBody]ISM_PlannerCreationDTO data)
        {
            return _objinter.TriggerPlannerMail(data);
        }


    }
}
