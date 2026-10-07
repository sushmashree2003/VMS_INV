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
    public class ISM_NotificationController : Controller
    {

        public ISM_NotificationDelegate _objdel = new ISM_NotificationDelegate();

        [Route("getdetails/{id:int}")]
        public ISM_NotificationDTO getdetails(int id)
        {
            ISM_NotificationDTO data = new ISM_NotificationDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));        
            return _objdel.getdetails(data);
        }

        [Route("getnotificationdetails")]
        public ISM_NotificationDTO getnotificationdetails([FromBody]ISM_NotificationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));           
            return _objdel.getnotificationdetails(data);
        }

        [Route("pushnotificationdetails")]
        public ISM_NotificationDTO pushnotificationdetails([FromBody]ISM_NotificationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.pushnotificationdetails(data);
        }
    }
}
