using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.SpaceBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.SpaceBooking;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.SpaceBooking
{
    [Route("api/[controller]")]
    public class SBReportController : Controller
    {
        public SBReportDelegate _delobj = new SBReportDelegate();

        [Route("getdetails/{id:int}")]
        public SBReport_DTO getdetails(int id)
        {
            SBReport_DTO data = new SBReport_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.getdetails(data);
        }

        [Route("registration_report")]
        public SBReport_DTO registration_report([FromBody]SBReport_DTO data)
        {            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.registration_report(data);
        }

        [Route("enquary_report")]
        public SBReport_DTO enquary_report([FromBody]SBReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delobj.enquary_report(data);
        }

        [Route("booking_report")]
        public SBReport_DTO booking_report([FromBody]SBReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.booking_report(data);
        }

        [Route("getroom")]
        public SBReport_DTO getroom([FromBody]SBReport_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getroom(data);
        }
    }
}
