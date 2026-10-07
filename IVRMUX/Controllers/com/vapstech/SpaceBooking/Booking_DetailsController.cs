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
    public class Booking_DetailsController : Controller
    {
        // GET: api/<controller>
        public Booking_DetailsDelegate _delobj = new Booking_DetailsDelegate();

        [Route("getdetails/{id:int}")]
        public Booking_DetailsDTO getdetails(int id)
        {
            Booking_DetailsDTO data = new Booking_DetailsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }
        [Route("booking_report")]
        public Booking_DetailsDTO booking_report([FromBody]Booking_DetailsDTO data)
       {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.booking_report(data);
        }
    }
}
