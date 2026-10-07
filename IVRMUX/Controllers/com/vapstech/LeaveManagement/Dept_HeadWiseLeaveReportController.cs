using IVRMUX.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.LeaveManagement
{
    [Route("api/[controller]")]
    public class Dept_HeadWiseLeaveReportController : Controller
    {
        Dept_HeadWiseLeaveReportDelegate lcd = new Dept_HeadWiseLeaveReportDelegate();

        // GET: api/values
        [Route("getdatedetails")]
        public Dept_HeadWiseLeaveReportDTO getdatedetails([FromBody] Dept_HeadWiseLeaveReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getdatedetails(data);
        }
    }
}
