using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class InOutBoundCallDetailController : Controller
    {
        InOutBoundCallDetailDelegate od = new InOutBoundCallDetailDelegate();
        
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public ISM_Inbound_Outbound_Call_DetailsDTO getinitialdropdowns(int id)
        {
            ISM_Inbound_Outbound_Call_DetailsDTO data = new ISM_Inbound_Outbound_Call_DetailsDTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getdata(data);
        }

        [HttpPost]
        [Route("report")]
        public ISM_Inbound_Outbound_Call_DetailsDTO report([FromBody] ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.report(data);
        }
        [HttpPost]
        [Route("savedata")]
        public ISM_Inbound_Outbound_Call_DetailsDTO savedata([FromBody] ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedata(data);
        }

        [Route("getdetails/{id:int}")]
        public ISM_Inbound_Outbound_Call_DetailsDTO getdetails(int id)
        {
            return od.getdetails(id);
        }

        [Route("deletedata/{id:int}")]
        public ISM_Inbound_Outbound_Call_DetailsDTO deletedata(int id)
        {
            return od.deletedata(id);
        }
    }
}
