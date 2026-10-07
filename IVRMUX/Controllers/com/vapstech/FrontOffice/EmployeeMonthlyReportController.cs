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
    public class EmployeeMonthlyReportController : Controller
    {
        EmployeeMonthlyReportDelegate od = new EmployeeMonthlyReportDelegate();
        
        [Route("getalldetails/{id:int}")]
        public EmployeeMonthlyReportDTO getinitialdropdowns(int id)
        {
            EmployeeMonthlyReportDTO data = new EmployeeMonthlyReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return od.getdata(data);
        }
        [HttpPost]
        [Route("get_departments")]
        public EmployeeMonthlyReportDTO get_departments([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_departments(data);
        }

        [Route("get_designation")]
        public EmployeeMonthlyReportDTO get_designation([FromBody] EmployeeMonthlyReportDTO data)
        {            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_designation(data);
        }
        [Route("get_employee")]
        public EmployeeMonthlyReportDTO get_employee([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_employee(data);
        }
        
        [Route("get_employeeselected")]
        public EmployeeMonthlyReportDTO get_employeeselected([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_employeeselected(data);
        }

        [HttpPost]
        [Route("getrpt")]
        public EmployeeMonthlyReportDTO getrpt([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getrpt(data);
        }

        [Route("getalldetailsselected/{id:int}")]
        public EmployeeMonthlyReportDTO getalldetailsselected(int id)
        {
            EmployeeMonthlyReportDTO data = new EmployeeMonthlyReportDTO();
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.getalldetailsselected(data);
        }

        [HttpPost]
        [Route("get_departmentselected")]
        public EmployeeMonthlyReportDTO get_departmentselected([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_departmentselected(data);
        }

        [HttpPost]
        [Route("get_designationselected")]
        public EmployeeMonthlyReportDTO get_designationselected([FromBody] EmployeeMonthlyReportDTO data)
        {
            data.LoginId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return od.get_designationselected(data);
        }
    }
}
