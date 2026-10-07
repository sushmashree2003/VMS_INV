using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using IVRMUX.Delegates.com.vapstech.HRMS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class employeeadvanceApplyController : Controller
    {
        emloyeeadvanceApplyDelegate del = new emloyeeadvanceApplyDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HR_Emp_SalaryAdvanceDTO getalldetails(int id)
        {
            HR_Emp_SalaryAdvanceDTO dto = new HR_Emp_SalaryAdvanceDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        // POST api/values
        [HttpPost]
        public HR_Emp_SalaryAdvanceDTO Post([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }
        [Route("Edit")]
        public HR_Emp_SalaryAdvanceDTO Edit([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.Edit(dto);
        }


        [Route("ActiveDeactiveRecord/{id:int}")]
        public HR_Emp_SalaryAdvanceDTO ActiveDeactiveRecord(int id)
        {
            HR_Emp_SalaryAdvanceDTO dto = new HR_Emp_SalaryAdvanceDTO();
            dto.HRESA_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleterec(dto);
        }
        [Route("getDetailsByEmployee")]
        public HR_Emp_SalaryAdvanceDTO getDetailsByEmployee([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getDetailsByEmployee(dto);
        }

    }
}


