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
    public class MasterActivitiesFacade : Controller
    {

        public MasterActivitiesInterface _objinter;

        public MasterActivitiesFacade(MasterActivitiesInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public MasterActivities_DTO getdetails([FromBody] MasterActivities_DTO dTO)
        {
            return _objinter.getdetails(dTO);
        }

        [Route("saverecord")]
        public MasterActivities_DTO saverecord([FromBody]MasterActivities_DTO value)
        {
            return _objinter.saverecord(value);
        }

        [Route("deactiveY")]
        public MasterActivities_DTO deactiveY([FromBody]MasterActivities_DTO value)
        {
            return _objinter.deactiveY(value);
        }



    }
}
