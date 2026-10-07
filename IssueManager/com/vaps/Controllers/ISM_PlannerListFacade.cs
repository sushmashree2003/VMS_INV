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
    public class ISM_PlannerListFacade : Controller
    {

        public ISM_PlannerListInterface _objinter;

        public ISM_PlannerListFacade(ISM_PlannerListInterface parameter)
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
     



    }
}
