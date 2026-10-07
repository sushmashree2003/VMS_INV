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
    public class RoomBankDeatilsController : Controller
    {
        RoomBankDeatilsDelegate _comm = new RoomBankDeatilsDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("getloaddata/{id:int}")]
        public RoomBankDeatilsDTO getloaddata(int id)
        {
            RoomBankDeatilsDTO data = new RoomBankDeatilsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _comm.getloaddata(data);
        }




        [Route("savedetails")]
        public RoomBankDeatilsDTO savedetails([FromBody]RoomBankDeatilsDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _comm.savedetails(data);
        }

        [Route("deactive")]
        public RoomBankDeatilsDTO deactive([FromBody] RoomBankDeatilsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _comm.deactive(data);
        }
    }
}

