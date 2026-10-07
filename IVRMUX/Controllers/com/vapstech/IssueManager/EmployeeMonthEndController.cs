using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class EmployeeMonthEndController : Controller
    {
        EmployeeMonthEndDelegate del = new EmployeeMonthEndDelegate();

        [Route("getemplist/{id:int}")]
        public EmployeeMonthEndDTO getverifyemplist(int id)
        {
            EmployeeMonthEndDTO obj = new EmployeeMonthEndDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getemplist(obj);
        }

        [Route("getempdetails")]
        public EmployeeMonthEndDTO getempdetails([FromBody] EmployeeMonthEndDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getempdetails(data);
        }

        [Route("savereport")]
        public EmployeeMonthEndDTO savereport([FromBody] EmployeeMonthEndDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.savereport(data);
        }
    }
}
