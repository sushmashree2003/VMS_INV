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
    public class GPSLocationController : Controller
    {
        GPSLocationDelegate od = new GPSLocationDelegate();
        
        [HttpPost]
        [Route("getalldetails")]
        public VMS_Attendance_GPSLocationDTO getinitialdropdowns([FromBody] VMS_Attendance_GPSLocationDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getdata(data);
        }
            
        [HttpPost]
        [Route("save")]
        public VMS_Attendance_GPSLocationDTO savedata([FromBody] VMS_Attendance_GPSLocationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return od.savedata(data);
        }
    }
}
