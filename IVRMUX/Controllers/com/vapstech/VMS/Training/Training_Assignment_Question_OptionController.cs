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
    public class Training_Assignment_Question_OptionController : Controller
    {
        Training_Assignment_Question_Option_Delegate TMD = new Training_Assignment_Question_Option_Delegate();
        [HttpGet]
        [Route("getdata_TAQO/{id:int}")]
        public HR_Training_Assignment_Question_Option_DTO getdata_TAQO(int id)
        {
            HR_Training_Assignment_Question_Option_DTO dto = new HR_Training_Assignment_Question_Option_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_TAQO(dto);
        }

        [Route("SaveEdit_TAQO")]
        public HR_Training_Assignment_Question_Option_DTO SaveEdit_TAQO([FromBody] HR_Training_Assignment_Question_Option_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.SaveEdit_TAQO(dto);
        }
        [HttpPost]
        [Route("deactivate_TAQO")]
        public HR_Training_Assignment_Question_Option_DTO deactivate_TAQO([FromBody] HR_Training_Assignment_Question_Option_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.deactivate_TAQO(dto);
        }

        [Route("details_TAQO/{id:int}")]
        public HR_Training_Assignment_Question_Option_DTO details_TAQO(int id)
        {
            HR_Training_Assignment_Question_Option_DTO dto = new HR_Training_Assignment_Question_Option_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRTAQNS_Id = id;
            return TMD.details_TAQO(dto);
        }

        [Route("option_view_TAQO/{id:int}")]
        public HR_Training_Assignment_Question_Option_DTO option_view_MQO(int id)
        {
            HR_Training_Assignment_Question_Option_DTO dto = new HR_Training_Assignment_Question_Option_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRTAQNS_Id = id;
            return TMD.option_view_TAQO(dto);
        }


    }
}
