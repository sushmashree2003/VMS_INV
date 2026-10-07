using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.DriversChart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.DriversChart;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.DriversChart
{
    [Route("api/[controller]")]
    public class Master_Driver_ShiftsController : Controller
    {
        Master_Driver_Shifts_Delegate del = new Master_Driver_Shifts_Delegate();

        [Route("getdetails/{id:int}")]
        public DriverShiftsDTO getdetails(int id)
        {
            DriverShiftsDTO data = new DriverShiftsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdetails(data);
        }
        [HttpPost]
        [Route("savedata")]
        public DriverShiftsDTO savedata([FromBody]DriverShiftsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }

        [Route("EditData/{id:int}")]
        public DriverShiftsDTO Editdata(int id)
        {
            DriverShiftsDTO data = new DriverShiftsDTO();
            data.VMSMDSH_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
    }
}
