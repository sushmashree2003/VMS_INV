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
    public class VenueMappingFacade : Controller
    {

        public VenueMappingInterface _objinter;

        public VenueMappingFacade(VenueMappingInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public VenueMapping_DTO getdetails([FromBody] VenueMapping_DTO dTO)
        {
            return _objinter.getdetails(dTO);
        }

        [Route("saverecord")]
        public VenueMapping_DTO saverecord([FromBody]VenueMapping_DTO value)
        {
            return _objinter.saverecord(value);
        }

        [Route("deactiveY")]
        public VenueMapping_DTO deactiveY([FromBody]VenueMapping_DTO value)
        {
            return _objinter.deactiveY(value);
        }




    }
}
