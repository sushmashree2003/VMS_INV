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
    public class Training_Assignment_TopicController : Controller
    {
        Training_Assignment_Topic_Delegate TMD = new Training_Assignment_Topic_Delegate();

        [HttpGet]
        [Route("getdata_TAT/{id:int}")]
        public HR_Training_Assignment_Topic_DTO getdata_TAQO(int id)
        {
            HR_Training_Assignment_Topic_DTO dto = new HR_Training_Assignment_Topic_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return TMD.getdata_TAT(dto);
        }

    }
}
