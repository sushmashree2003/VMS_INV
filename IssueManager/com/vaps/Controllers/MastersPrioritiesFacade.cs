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
    public class MastersPrioritiesFacade : Controller
    {

        public MastersPrioritiesInterface _objinter;

        public MastersPrioritiesFacade(MastersPrioritiesInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public MastersPriorities_DTO getdetails([FromBody] MastersPriorities_DTO dTO)
        {
            return _objinter.getdetails(dTO);
        }

        [Route("saverecord")]
        public MastersPriorities_DTO saverecord([FromBody]MastersPriorities_DTO value)
        {
            return _objinter.saverecord(value);
        }

        [Route("deactiveY")]
        public MastersPriorities_DTO deactiveY([FromBody]MastersPriorities_DTO value)
        {
            return _objinter.deactiveY(value);
        }




    }
}
