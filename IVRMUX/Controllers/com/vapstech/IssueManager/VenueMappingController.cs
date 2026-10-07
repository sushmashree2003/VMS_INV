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
    public class VenueMappingController : Controller
    {


        public VenueMappingDelegate _objdel = new VenueMappingDelegate();

        [Route("getdetails/{id:int}")]
        public VenueMapping_DTO getdetails(int id)
        {
            VenueMapping_DTO dTO = new VenueMapping_DTO();
            dTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getdetails(dTO);
        }

        [Route("saverecord")]
        public VenueMapping_DTO saverecord([FromBody]VenueMapping_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saverecord(value);
        }

        [Route("deactiveY")]
        public VenueMapping_DTO deactiveY([FromBody]VenueMapping_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deactiveY(value);
        }





    }
}
