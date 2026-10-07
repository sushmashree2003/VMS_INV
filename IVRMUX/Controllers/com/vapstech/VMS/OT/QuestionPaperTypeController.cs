using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using corewebapi18072016.Delegates.com.vapstech.VMS.OT;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.VMS.OT
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class QuestionPaperTypeController : Controller
    {
        QuestionPaperTypeDelegate del = new QuestionPaperTypeDelegate(); 
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public QuestionPaperTypeDTO getalldetails(int id)
        {
            QuestionPaperTypeDTO dto = new QuestionPaperTypeDTO();
          //  dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.getalldetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Route("savedata")]
        public QuestionPaperTypeDTO savedetails([FromBody]QuestionPaperTypeDTO dto)
        {
         //   dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.savedetails(dto);
        }

        [HttpPost]
        [Route("loadcdata")]
        public QuestionPaperTypeDTO loadcdata([FromBody]QuestionPaperTypeDTO dto)
        {
         //   dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return del.loadcdata(dto);
        }

        [Route("editData/{id:int}")]
        public QuestionPaperTypeDTO editData(int id)
        {
            return del.editData(id);
        }

        [Route("deactivate")]
        public QuestionPaperTypeDTO deactivate([FromBody]QuestionPaperTypeDTO dto)
        {
          //  dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactivate(dto);
        }
    }
}
