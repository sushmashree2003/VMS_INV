using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class VMS_SalaryCheck_ApprovalController : Controller
    {
        VMS_SalaryCheck_ApprovalDelegate del = new VMS_SalaryCheck_ApprovalDelegate();
        // GET: api/values
        [HttpGet]
        [Route("approvalloaddata/{id:int}")]
        public HR_Employee_Salary_PaymentDetailsDTO getalldetails(int id)
        {
            HR_Employee_Salary_PaymentDetailsDTO dto = new HR_Employee_Salary_PaymentDetailsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [Route("findchecklist")]
        public HR_Employee_Salary_PaymentDetailsDTO getEmployeedetailsBySelection([FromBody]HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeedetailsBySelection(dto);
        }

        [Route("saveapproval")]
        public HR_Employee_Salary_PaymentDetailsDTO saveapproval([FromBody]HR_Employee_Salary_PaymentDetailsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saveapproval(dto);
        }
    }
}
