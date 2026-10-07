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
    public class Training_Assignment_OptionController : Controller
    {
        Training_Assignment_Option_Delegate TMD = new Training_Assignment_Option_Delegate();
        [HttpGet]
        [Route("getdata_MFO/{id:int}")]
        public HR_Training_Assignment_Option_DTO getdata_MFO(int id)
        {
            HR_Training_Assignment_Option_DTO dto = new HR_Training_Assignment_Option_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_MFO(dto);
        }

        [Route("SaveEdit_MFO")]
        public HR_Training_Assignment_Option_DTO SaveEdit_MFO([FromBody] HR_Training_Assignment_Option_DTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.SaveEdit_MFO(dto);
        }

        [HttpPost]
        [Route("deactivate_MFO")]
        public HR_Training_Assignment_Option_DTO deactivate_MFO([FromBody] HR_Training_Assignment_Option_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.deactivate_MFO(dto);
        }


        [Route("details_MFO/{id:int}")]
        public HR_Training_Assignment_Option_DTO details_MFO(int id)
        {
            HR_Training_Assignment_Option_DTO dto = new HR_Training_Assignment_Option_DTO();
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRTAOPT_Id = id;
            return TMD.details_MFO(dto);
        }



    }
}
    
