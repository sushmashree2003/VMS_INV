using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FMS
{
    [Route("api/[controller]")]
    public class ModuleFeedbackController : Controller
    {
        ModuleFeedbackDelegate MFD = new ModuleFeedbackDelegate();

        [Route("getfeedback")]
        public ModuleFeedbackDTO getfeedback([FromBody] ModuleFeedbackDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return MFD.getfeedback(data);
        }
        [Route("submitfeedback")]
        public ModuleFeedbackDTO submitfeedback([FromBody] ModuleFeedbackDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return MFD.submitfeedback(data);
        }
        [Route("checkfeedback")]
        public ModuleFeedbackDTO checkfeedback([FromBody] ModuleFeedbackDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return MFD.checkfeedback(data);
        }
    }
}
