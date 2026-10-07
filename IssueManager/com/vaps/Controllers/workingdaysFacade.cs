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
    public class workingdaysFacade : Controller
    {
        public workingdaysInterface _objinter;

        public workingdaysFacade(workingdaysInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public workingdays_DTO getdetails([FromBody] workingdays_DTO dTO)
        {
            return _objinter.getdetails(dTO);
        }

        [Route("saverecord")]
        public workingdays_DTO saverecord([FromBody]workingdays_DTO value)
        {
            return _objinter.saverecord(value);
        }

        [Route("deactiveY")]
        public workingdays_DTO deactiveY([FromBody]workingdays_DTO value)
        {
            return _objinter.deactiveY(value);
        }
        [Route("editrecord")]
        public workingdays_DTO editrecord([FromBody]workingdays_DTO value)
        {
            return _objinter.editrecord(value);
        }
        

    }
}
