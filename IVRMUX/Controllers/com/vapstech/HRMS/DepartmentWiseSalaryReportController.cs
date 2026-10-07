using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;
using PreadmissionDTOs.com.vaps.HRMS;
using IVRMUX.Delegates.com.vapstech.HRMS;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class DepartmentWiseSalaryReportController : Controller
    {
        DepartmentWiseSalaryReportDelegate objdelegate1 = new DepartmentWiseSalaryReportDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public DepartmentWiseSalaryReportDTO getalldetails(int id)
        {
            DepartmentWiseSalaryReportDTO data = new DepartmentWiseSalaryReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return objdelegate1.getalldetails(data);
        }

        [Route("getdaily_data")]
        public DepartmentWiseSalaryReportDTO getdaily_data([FromBody]DepartmentWiseSalaryReportDTO data)
        {


            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate1.getdaily_data(data);
        }
        [HttpGet]
        [Route("getsalaryalldetails/{id:int}")]
        public DepartmentWiseSalaryReportDTO getsalaryalldetails(int id)
        {
            DepartmentWiseSalaryReportDTO data = new DepartmentWiseSalaryReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate1.getsalaryalldetails(data);
        }

        [Route("get_depts")]
        public DepartmentWiseSalaryReportDTO get_depts([FromBody]DepartmentWiseSalaryReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
             dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate1.get_depts(dto);
        }

        [Route("Getreport")]
        public DepartmentWiseSalaryReportDTO Getreport([FromBody]DepartmentWiseSalaryReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
             dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return objdelegate1.Getreport(dto);
        }
    }
}
