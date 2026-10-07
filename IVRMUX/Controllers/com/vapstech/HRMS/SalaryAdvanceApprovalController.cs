using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SalaryAdvanceApprovalController : Controller
    {
        SalaryAdvanceApprovalDelegate del = new SalaryAdvanceApprovalDelegate();
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

        [Route("empdetails")]
        public HR_Emp_SalaryAdvanceDTO empdetails([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.empdetails(dto);
        }


        [Route("approvedrecord")]
        public HR_Emp_SalaryAdvanceDTO approvedrecord([FromBody] HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.approvedrecord(dto);
        }

        //------------------ Advance Approval Report------------------------------
        [Route("onloaddata/{id:int}")]
        public HR_Emp_SalaryAdvanceDTO Onload(int n)
        {
            HR_Emp_SalaryAdvanceDTO dto = new HR_Emp_SalaryAdvanceDTO();
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Onload(dto);
        }
        [HttpPost]
        [Route("getapprovalreport")]
        public HR_Emp_SalaryAdvanceDTO getapprovalreport([FromBody]HR_Emp_SalaryAdvanceDTO dto)
        {
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getapprovalreport(dto);
        }




    }
}