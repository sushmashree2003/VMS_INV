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
    public class ModuleWiseFeedbackReportController : Controller
    {
        public ModuleWiseFeedbackReportDelegate _objdel = new ModuleWiseFeedbackReportDelegate();

        [Route("OnLoadClient/{id:int}")]
        public ModuleWiseFeedbackReport_DTO OnLoadClient(int id)
        {
            ModuleWiseFeedbackReport_DTO data = new ModuleWiseFeedbackReport_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.OnLoadClient(data);
        }

        [Route("getmodulewisefeedbackreport")]
        public ModuleWiseFeedbackReport_DTO getmodule([FromBody]ModuleWiseFeedbackReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getfeedbackreport(data);
        }
        [Route("getvmsmodulefeedbackreport")]
        public ModuleWiseFeedbackReport_DTO getvmsmodulefeedbackreport([FromBody]ModuleWiseFeedbackReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getvmsmodulefeedbackreport(data);
        }
        [Route("getcanteenandhousekeepingfeedback")]
        public ModuleWiseFeedbackReport_DTO getcanteenandhousekeepingfeedback([FromBody]ModuleWiseFeedbackReport_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getcanteenandhousekeepingfeedback(data);
        }

    }
}
