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
    public class ISM_PrioritySwitchReportController : Controller
    {

        public ISM_PrioritySwitchReportDelegate _objdel = new ISM_PrioritySwitchReportDelegate();

        [Route("getdetails/{id:int}")]
        public ISM_PrioritySwitchReportDTO getdetails(int id)
        {
            ISM_PrioritySwitchReportDTO data = new ISM_PrioritySwitchReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getdetails(data);
        }

        [Route("getdeviationReport")]
        public ISM_PrioritySwitchReportDTO getdeviationReport([FromBody]ISM_PrioritySwitchReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.getdeviationReport(data);
        }

        //======================weekly deviation

        [Route("getdetails_weekly/{id:int}")]
        public ISM_PrioritySwitchReportDTO getdetails_weekly(int id)
        {
            ISM_PrioritySwitchReportDTO data = new ISM_PrioritySwitchReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
         
            return _objdel.getdetails(data);
        }
        [Route("getdeviationReport_weekly")]
        public ISM_PrioritySwitchReportDTO getdeviationReport_weekly([FromBody]ISM_PrioritySwitchReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getdeviationReport_weekly(data);
        }

        //=====================rating report==========


        [Route("getEmp_ratingReport")]
        public ISM_PrioritySwitchReportDTO getEmp_ratingReport([FromBody]ISM_PrioritySwitchReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.getEmp_ratingReport(data);
        }
    }
}
