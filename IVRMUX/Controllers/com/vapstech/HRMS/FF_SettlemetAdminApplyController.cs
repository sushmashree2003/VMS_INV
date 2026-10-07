using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FF_SettlemetAdminApplyController : Controller
    {
        // GET: /<controller>/
        FF_SettlemetAdminApplyDelegate del = new FF_SettlemetAdminApplyDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FF_Settlement_Process_DTO getalldetails(int id)
        {
            FF_Settlement_Process_DTO dto = new FF_Settlement_Process_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }
        // POST api/values
        [HttpPost]
        public FF_Settlement_Process_DTO Post([FromBody] FF_Settlement_Process_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(dto);
        }

        [Route("editRecord/{id:int}")]
        public FF_Settlement_Process_DTO editRecord(int id)
        {
            FF_Settlement_Process_DTO dto = new FF_Settlement_Process_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.getRecorddetailsById(id);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public FF_Settlement_Process_DTO ActiveDeactiveRecord(int id)
        {
            FF_Settlement_Process_DTO dto = new FF_Settlement_Process_DTO();
            dto.HRESA_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deleterec(dto);
        }


        [Route("getDetailsByEmployee")]
        public FF_Settlement_Process_DTO getDetailsByEmployee([FromBody] FF_Settlement_Process_DTO dto)
        {            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getDetailsByEmployee(dto);
        }

        [Route("searchfilter")]
        public FF_Settlement_Process_DTO searchfilter([FromBody] FF_Settlement_Process_DTO dto)

        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.searchfilter(dto);
        }

        [Route("empdetails")]
        public FF_Settlement_Process_DTO empdetails([FromBody] FF_Settlement_Process_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.empdetails(dto);
        }
    }
}
