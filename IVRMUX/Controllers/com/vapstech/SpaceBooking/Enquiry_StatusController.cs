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
    public class Enquiry_StatusController : Controller
    {
        Enquiry_StatusDelegate del = new Enquiry_StatusDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }       
        [Route("load_data")]
        public Enquiry_StatusDTO load_data([FromBody]Enquiry_StatusDTO data)
        {         
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));         
            return del.load_data(data);
        }
        [Route("ClickBookVisit")]
        public Enquiry_StatusDTO ClickBookVisit([FromBody]Enquiry_StatusDTO data)
        {          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return del.ClickBookVisit(data);
        }
        [Route("SendStatus")]
        public Enquiry_StatusDTO SendStatus([FromBody]Enquiry_StatusDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));            
            return del.SendStatus(data);
        }
        [Route("getedit")]
        public Enquiry_StatusDTO getedit([FromBody]Enquiry_StatusDTO dto)
        {           
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));          
            return del.getedit(dto);
        }
    }
}
