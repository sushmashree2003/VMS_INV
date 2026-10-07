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
    public class ISM_SwapIssueReportController : Controller
    {
        public ISM_SwapIssueReportDelegate _objdel = new ISM_SwapIssueReportDelegate();

        [Route("getdetails/{id:int}")]
        public ISM_PlannerReportsDTO getdetails(int id)
        {
            ISM_PlannerReportsDTO data = new ISM_PlannerReportsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getdetails(data);
        }

        [Route("getdeviationReport")]
        public ISM_PlannerReportsDTO getdeviationReport([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.getdeviationReport(data);
        }

        //======================weekly deviation

        [Route("getdetails_weekly/{id:int}")]
        public ISM_PlannerReportsDTO getdetails_weekly(int id)
        {
            ISM_PlannerReportsDTO data = new ISM_PlannerReportsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getdetails(data);
        }
        [Route("getdeviationReport_weekly")]
        public ISM_PlannerReportsDTO getdeviationReport_weekly([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getdeviationReport_weekly(data);
        }

        //=====================rating report==========


      
    }
}
