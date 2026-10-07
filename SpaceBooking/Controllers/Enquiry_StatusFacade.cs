using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.SpaceBooking;
using SpaceBooking.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpaceBooking.Controllers
{
    [Route("api/[controller]")]
    public class Enquiry_StatusFacade : Controller
    {
        public Enquiry_StatusInterface _ads;
        public Enquiry_StatusFacade(Enquiry_StatusInterface adstu)
        {
            _ads = adstu;
        }
        [Route("load_data")]
        public Enquiry_StatusDTO load_data([FromBody]Enquiry_StatusDTO data)
        {
            return _ads.load_data(data);
        }
        [Route("ClickBookVisit")]
        public Enquiry_StatusDTO ClickBookVisit([FromBody]Enquiry_StatusDTO data)
        {
            return _ads.ClickBookVisit(data);
        }
        [Route("SendStatus")]
        public Enquiry_StatusDTO SendStatus([FromBody]Enquiry_StatusDTO data)
        {
            return _ads.SendStatus(data);
        }
        [Route("getedit")]
        public Enquiry_StatusDTO getedit([FromBody]Enquiry_StatusDTO data)
        {
            return _ads.getedit(data);
        }
    }
}
