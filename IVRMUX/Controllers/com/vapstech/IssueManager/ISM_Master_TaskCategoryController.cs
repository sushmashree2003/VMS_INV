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
    public class ISM_Master_TaskCategoryController : Controller
    {
        ISM_Master_TaskCategoryDelegate del = new ISM_Master_TaskCategoryDelegate();
        [Route("loaddata/{id:int}")]
        public ISM_Master_TaskCategory_DTO loaddata(int id)
        {
            ISM_Master_TaskCategory_DTO data = new ISM_Master_TaskCategory_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("save")]
        public ISM_Master_TaskCategory_DTO save([FromBody]ISM_Master_TaskCategory_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
        [Route("deactive")]
        public ISM_Master_TaskCategory_DTO deactive([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public ISM_Master_TaskCategory_DTO EditData([FromBody] ISM_Master_TaskCategory_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(data);
        }
        //============== Master Task Categary Check List===========
        [Route("loaddata_checklist/{id:int}")]
        public ISM_Master_TaskCategory_DTO loaddata_checklist(int id)
        {
            ISM_Master_TaskCategory_DTO dto = new ISM_Master_TaskCategory_DTO();
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata_checklist(dto);
        }
        [Route("getcategory")]
        public ISM_Master_TaskCategory_DTO getcategory([FromBody] ISM_Master_TaskCategory_DTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return del.getcategory(dto);
        }
         [Route("savechecklist")]
        public ISM_Master_TaskCategory_DTO savechecklist([FromBody] ISM_Master_TaskCategory_DTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
            return del.savechecklist(dto);
        }
        [Route("EditData_checklist")]
        public ISM_Master_TaskCategory_DTO EditData_checklist([FromBody] ISM_Master_TaskCategory_DTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData_checklist(dto);
        }
        [Route("deactive_checklist")]
        public ISM_Master_TaskCategory_DTO deactive_checklist([FromBody] ISM_Master_TaskCategory_DTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive_checklist(dto);
        }


    }
}
