using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.Tenants
{
    [Route("api/[controller]")]
    public class RoomWiseAmenitiesMappingController : Controller
    {
        RoomWiseAmenitiesMappingDelegate _comm = new RoomWiseAmenitiesMappingDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("getloaddata/{id:int}")]
        public RoomWiseAmenitiesMappingDTO getloaddata(int id)
        {
            RoomWiseAmenitiesMappingDTO data = new RoomWiseAmenitiesMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _comm.getloaddata(data);
        }




        [Route("savedetails")]
        public RoomWiseAmenitiesMappingDTO savedetails([FromBody]RoomWiseAmenitiesMappingDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _comm.savedetails(data);
        }

        [Route("deactive")]
        public RoomWiseAmenitiesMappingDTO deactive([FromBody] RoomWiseAmenitiesMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _comm.deactive(data);
        }

        [Route("Edit")]
        public RoomWiseAmenitiesMappingDTO Edit([FromBody] RoomWiseAmenitiesMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _comm.Edit(data);
        }
    }
}
