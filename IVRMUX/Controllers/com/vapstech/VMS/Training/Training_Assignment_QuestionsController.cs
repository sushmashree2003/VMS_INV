using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.Training;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.Training
{
    [Route("api/[controller]")]
    public class Training_Assignment_QuestionsController : Controller
    {
        // GET: api/<controller>
        Training_Assignment_Questions_Delegate TMD = new Training_Assignment_Questions_Delegate();
        [HttpGet]
        [Route("getdata_question/{id:int}")]
        public HR_Training_Assignment_Questions_DTO getdata_question(int id)
        {
            HR_Training_Assignment_Questions_DTO dto = new HR_Training_Assignment_Questions_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_question(dto);
        }
        [Route("SaveEdit_question")]
        public HR_Training_Assignment_Questions_DTO SaveEdit_question([FromBody] HR_Training_Assignment_Questions_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.SaveEdit_question(dto);
        }

        [Route("details_question/{id:int}")]
        public HR_Training_Assignment_Questions_DTO details_question(int id)
        {
            HR_Training_Assignment_Questions_DTO dto = new HR_Training_Assignment_Questions_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRTAQNS_Id = id;
            return TMD.details_question(dto);
        }


        [HttpPost]
        [Route("deactivate_question")]
        public HR_Training_Assignment_Questions_DTO deactivate_question([FromBody] HR_Training_Assignment_Questions_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.deactivate_question(dto);
        }


    }
}
