using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class EmployeeLeaveApplyController : Controller
    {
        EmployeeLeaveApplyDelegate lcd = new EmployeeLeaveApplyDelegate();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        // GET: api/values


        [Route("getonlineLeave/{id:int}")]
        public EmployeeDashboardDTO getonlineLeave(int id)
        {
            EmployeeDashboardDTO lv = new EmployeeDashboardDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getonlineLeave(lv);
        }

        [Route("save")]
        public EmployeeDashboardDTO onlineleavesave([FromBody] EmployeeDashboardDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.savedetail(test);
        }
    }
}
