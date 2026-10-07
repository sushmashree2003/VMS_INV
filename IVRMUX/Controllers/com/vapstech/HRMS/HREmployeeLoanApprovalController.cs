using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class HREmployeeLoanApprovalController : Controller
    {
        HREmployeeLoanApprovalDelegate del = new HREmployeeLoanApprovalDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HREmployeeLoanApprovalDTO getalldetails(int id)
        {
            HREmployeeLoanApprovalDTO dto = new HREmployeeLoanApprovalDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("getEmployeedetailsBySelection")]
        public HREmployeeLoanApprovalDTO getEmployeedetailsBySelection([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getEmployeedetailsBySelection(dto);
        }

        [Route("get_depts")]
        public HREmployeeLoanApprovalDTO get_depts([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_depts(dto);
        }

        [Route("get_desig")]
        public HREmployeeLoanApprovalDTO get_desig([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_desig(dto);
        }

        [HttpPost]
        [Route("getapprvaldata")]
        public HREmployeeLoanApprovalDTO getapprvaldata([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getapprvaldata(dto);
        }

        [HttpPost]
        [Route("SalaryApproval")]
        public HREmployeeLoanApprovalDTO SalaryApproval([FromBody]HREmployeeLoanApprovalDTO dto)
        {
        dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.SalaryApproval(dto);
        }
        [HttpPost]
        [Route("SalaryApprovalgriddata")]
        public HREmployeeLoanApprovalDTO SalaryApprovalgriddata([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.SalaryApprovalgriddata(dto);
        }


        //F&F Report
        [HttpGet]
        [Route("getOnloaddata/{id:int}")]
        public HREmployeeLoanApprovalDTO getOnloaddata(int id)
        {
            HREmployeeLoanApprovalDTO dto = new HREmployeeLoanApprovalDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getOnloaddata(dto);
        }

        [HttpPost]
        [Route("getemployeedetails")]
        public HREmployeeLoanApprovalDTO getemployeedetails([FromBody]HREmployeeLoanApprovalDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getemployeedetails(dto);
        }

        [Route("ViewFiles")]
        public HREmployeeLoanApprovalDTO ViewFiles([FromBody] HREmployeeLoanApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.ViewFiles(data);
        }



    }
}
