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
    public class MasterVenuesFacade : Controller
    {

        public MasterVenuesInterface _objinter;

        public MasterVenuesFacade(MasterVenuesInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public MasterVenues_DTO getdetails([FromBody] MasterVenues_DTO dTO)
        {
            return _objinter.getdetails(dTO);
        }

        [Route("saverecord")]
        public MasterVenues_DTO saverecord([FromBody]MasterVenues_DTO value)
        {
            return _objinter.saverecord(value);
        }

        [Route("deactiveY")]
        public MasterVenues_DTO deactiveY([FromBody]MasterVenues_DTO value)
        {
            return _objinter.deactiveY(value);
        }



    }
}
