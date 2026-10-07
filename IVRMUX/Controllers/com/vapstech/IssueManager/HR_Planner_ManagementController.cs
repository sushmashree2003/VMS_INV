using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class HR_Planner_ManagementController : Controller
    {
        public HR_Planner_ManagementDelegate _objdel = new HR_Planner_ManagementDelegate();

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public HR_Planner_ManagementDTO getdetails(int id)
        {
            HR_Planner_ManagementDTO data = new HR_Planner_ManagementDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }
        [HttpGet]
        [Route("getdetailsreminder/{id:int}")]
        public HR_Planner_ManagementDTO getdetailsreminder(int id)
        {
            HR_Planner_ManagementDTO data = new HR_Planner_ManagementDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetailsreminder(data);
        }

        [HttpPost]
        [Route("savedata")]
        public HR_Planner_ManagementDTO savedata([FromBody]HR_Planner_ManagementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.savedata(data);
        }

        [Route("edit")]
        public HR_Planner_ManagementDTO edit([FromBody]HR_Planner_ManagementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.edit(data);
        }
        [Route("activedeactive")]
        public HR_Planner_ManagementDTO activedeactive([FromBody]HR_Planner_ManagementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.activedeactive(data);
        }

      


        [Route("viewuploadflies")]
        public HR_Planner_ManagementDTO viewuploadflies([FromBody]HR_Planner_ManagementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.viewuploadflies(data);
        }
        [Route("getempmailid")]
        public HR_Planner_ManagementDTO getempmailid([FromBody]HR_Planner_ManagementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));         
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getempmailid(data);
        }
        [Route("deleteuploadfile")]
        public HR_Planner_ManagementDTO deleteuploadfile([FromBody]HR_Planner_ManagementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.deleteuploadfile(data);
        }

        [HttpGet("DeleteYY/{id:int}")]
        public HR_Planner_ManagementDTO DeleteYY(int id)
        {
            HR_Planner_ManagementDTO obj = new HR_Planner_ManagementDTO();


            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            obj.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            obj.HRPLMG_Id = id;
            return _objdel.DeleteYY(obj);
        }
        [Route("clientDetails")]
        public HR_Planner_ManagementDTO clientDetails([FromBody]HR_Planner_ManagementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.clientDetails(data);
        }
        
    }
}
