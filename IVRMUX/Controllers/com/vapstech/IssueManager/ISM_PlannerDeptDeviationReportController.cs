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
    public class ISM_PlannerDeptDeviationReportController : Controller
    {

        public ISM_PlannerDeptDeviationReportDelegate _objdel = new ISM_PlannerDeptDeviationReportDelegate();

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

        [Route("Deptselectiondetails")]
        public ISM_PlannerReportsDTO Deptselectiondetails([FromBody]ISM_PlannerReportsDTO data)
        {            
            return _objdel.Deptselectiondetails(data);
        }
        [Route("DeptselectionHeaddetails")]
        public ISM_PlannerReportsDTO DeptselectionHeaddetails([FromBody]ISM_PlannerReportsDTO data)
        {
            return _objdel.DeptselectionHeaddetails(data);
        }
        [Route("gettaskdetails")]
        public ISM_PlannerReportsDTO gettaskdetails([FromBody]ISM_PlannerReportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.gettaskdetails(data);
        }
        [Route("gettaskdetailsreport")]
        public ISM_PlannerReportsDTO gettaskdetailsreport([FromBody]ISM_PlannerReportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.gettaskdetailsreport(data);
        }
        [Route("Desgselectiondetails")]
        public ISM_PlannerReportsDTO Desgselectiondetails([FromBody]ISM_PlannerReportsDTO data)
        {          
            return _objdel.Desgselectiondetails(data);
        }

        [Route("empcommunication")]
        public ISM_PlannerReportsDTO empcommunication([FromBody]ISM_PlannerReportsDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.empcommunication(data);
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

        //====================dailydeviation

        [Route("getdaily_deviation/{id:int}")]
        public ISM_PlannerReportsDTO getdaily_deviation(int id)
        {
            ISM_PlannerReportsDTO dto = new ISM_PlannerReportsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdaily_deviation(dto);
        }

       

    }
}
