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
    public class DriverOTController : Controller
    {
        DriverOTDelegate od = new DriverOTDelegate();
        
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public DriverOTDTO getinitialdropdowns(int id)
        {
            DriverOTDTO data = new DriverOTDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getdata(data);
        }

        [HttpPost]
        [Route("employeereport")]
        public DriverOTDTO employeereport([FromBody] DriverOTDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.employeereport(data);
        }
        [HttpPost]
        [Route("savedata")]
        public DriverOTDTO savedata([FromBody] DriverOTDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedata(data);
        }

        [Route("getdetails/{id:int}")]
        public DriverOTDTO getdetails(int id)
        {
            return od.getdetails(id);
        }

        [Route("deletedata/{id:int}")]
        public DriverOTDTO deletedata(int id)
        {
            return od.deletedata(id);
        }

        [HttpGet]
        [Route("getApprovalStatus/{id:int}")]
        public DriverOTDTO getApprovalStatus(int id)
        {
            DriverOTDTO data = new DriverOTDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.getApprovalStatus(data);
        }

        [HttpPost]
        [Route("get_status")]
        public DriverOTDTO get_status([FromBody] DriverOTDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.get_status(data);
        }

        [HttpPost]
        [Route("reject_status")]
        public DriverOTDTO reject_status([FromBody] DriverOTDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.reject_status(data);
        }

        [HttpPost]
        [Route("finalOTreport")]
        public DriverOTDTO finalOTreport([FromBody] DriverOTDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.finalOTreport(data);
        }
    }
}
