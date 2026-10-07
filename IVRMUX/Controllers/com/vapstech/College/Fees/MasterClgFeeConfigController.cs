using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.College.Fees;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.College.Fees
{
    [Route("api/[controller]")]
    public class MasterClgFeeConfigController : Controller
    {
        MasterClgFeeConfigDelegate FMD = new MasterClgFeeConfigDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MasterClgFeeConfigDTO Get([FromQuery] int id)
        {
            MasterClgFeeConfigDTO data = new MasterClgFeeConfigDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FMD.getdetailsY(data);
        }



        [HttpPost]
        public MasterClgFeeConfigDTO savedetail([FromBody] MasterClgFeeConfigDTO Grouppage)
        {
            Grouppage.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            Grouppage.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return FMD.savedetails(Grouppage);
        }

    }
}
