using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.HRMS
{
    [Route("api/[controller]")]

    public class AddResponseLiveChatBotController : Controller

    {
        //private readonly AddResponseLiveChatBotDelegate _service;
        AddResponseLiveChatBotDelegate adsd = new AddResponseLiveChatBotDelegate();

        //public AddResponseLiveChatBotController(AddResponseLiveChatBotDelegate service)
        //{
        //    _service = service;
        //}


        [Route("GetAllFAQs/{id:int}")]
        public AddResponseLiveChatBotDTO GetAllFAQs(int id)
        {
            AddResponseLiveChatBotDTO data = new AddResponseLiveChatBotDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            data.MI_Id = mid;
            return adsd.GetAllFAQs(data);
        }



        [Route("savedata")]
        public AddResponseLiveChatBotDTO savedata([FromBody]AddResponseLiveChatBotDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return adsd.savedata(data);
        }

        [Route("saveansdata")]
        public AddResponseLiveChatBotDTO saveansdata([FromBody]AddResponseLiveChatBotDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return adsd.saveansdata(data);
        }

        [Route("GetBotResponse")]
        public AddResponseLiveChatBotDTO GetBotResponse([FromBody] AddResponseLiveChatBotDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = UserId;
            return adsd.GetBotResponse(data);
        }


    }
}

