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
    public class FF_Settlement_ProcessController : Controller
    {
      
        FF_Settlement_Process_Delegate del = new FF_Settlement_Process_Delegate();
        
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public FF_Settlement_Process_DTO getalldetails(int id)
        {
            FF_Settlement_Process_DTO dto = new FF_Settlement_Process_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.getalldetails(dto);
        }
       
        [Route("SaveUpdate")]
        public FF_Settlement_Process_DTO SaveUpdate([FromBody] FF_Settlement_Process_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.SaveUpdate(dto);
        }

        [Route("Edit")]
        public FF_Settlement_Process_DTO Edit([FromBody] FF_Settlement_Process_DTO dto)
        {            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.Edit(dto);
        }

        [Route("ActiveDeactiveRecord/{id:int}")]
        public FF_Settlement_Process_DTO ActiveDeactiveRecord(int id)
        {
            FF_Settlement_Process_DTO dto = new FF_Settlement_Process_DTO();
            dto.HREFFS_Id = id;
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactivate(dto);
        }        
       
        
        ///////////////////////Approval////////////////////////////////
     
            [HttpGet]
            [Route("getdetails/{id:int}")]
            public FF_Settlement_Process_DTO getData(int id)
            {
            FF_Settlement_Process_DTO dto = new FF_Settlement_Process_DTO();
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
                dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
                return del.getData(dto);
            }
            // POST api/values

            [Route("empdetails")]
            public FF_Settlement_Process_DTO empdetail([FromBody] FF_Settlement_Process_DTO dto)
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
                dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
                return del.empdetail(dto);
            }
            [Route("approvedrecord")]
            public FF_Settlement_Process_DTO approvedrecord([FromBody] FF_Settlement_Process_DTO dto)
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
                dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
                return del.approvedrecord(dto);
            }

            //------------------ Advance Approval Report------------------------------
            [Route("onloaddata/{id:int}")]
            public FF_Settlement_Process_DTO Onload(int n)
            {
            FF_Settlement_Process_DTO dto = new FF_Settlement_Process_DTO();
                dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
                dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
                dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
                return del.Onload(dto);
            }
            [HttpPost]
            [Route("getapprovalreport")]
            public FF_Settlement_Process_DTO getapprovalreport([FromBody]FF_Settlement_Process_DTO dto)
            {
                dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
                dto.LogInUserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
                dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
                return del.getapprovalreport(dto);
            }
        
    }
}


