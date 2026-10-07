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
    public class ISM_ApprovalReportController : Controller
    {
        ISM_ApprovalReportDelegate _delg = new ISM_ApprovalReportDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }   
        [Route("getdetails/{id:int}")]
        public ISM_PlannerReportsDTO getdetails(ISM_PlannerReportsDTO data)
        {
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getdetails(data);
        }
          
        [Route("getPlannerlist")]
        public ISM_PlannerReportsDTO getPlannerlist([FromBody] ISM_PlannerReportsDTO data)
        {
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getPlannerlist(data);
        }
          
        [Route("getApprovalReport")]
        public ISM_PlannerReportsDTO getApprovalReport([FromBody]  ISM_PlannerReportsDTO data)
        {
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getApprovalReport(data);
        }
        [Route("getPlannerDReport")]
        public ISM_PlannerReportsDTO getPlannerDReport([FromBody]  ISM_PlannerReportsDTO data)
        {
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getPlannerDReport(data);
        }

        [Route("Deptselectiondetails")]
        public ISM_PlannerReportsDTO Deptselectiondetails([FromBody]ISM_PlannerReportsDTO data)
        {
            return _delg.Deptselectiondetails(data);
        }

        [Route("Desgselectiondetails")]
        public ISM_PlannerReportsDTO Desgselectiondetails([FromBody]ISM_PlannerReportsDTO data)
        {
            return _delg.Desgselectiondetails(data);
        }
        [Route("getempdetailsReport")]
        public ISM_PlannerReportsDTO getempdetailsReport([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delg.getempdetailsReport(data);
        }

        [Route("send_feedback")]
        public ISM_PlannerReportsDTO send_feedback([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.send_feedback(data);
        }

        [Route("viewresponse")]
        public ISM_PlannerReportsDTO viewresponse([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewresponse(data);
        }
        [Route("SummaryReport")]
        public ISM_PlannerReportsDTO SummaryReport([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SummaryReport(data);
        }


        [Route("getcatempdetails/{id:int}")]
        public ISM_PlannerReportsDTO getcatempdetails(ISM_PlannerReportsDTO data)
        {
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getcatempdetails(data);
        }

        [Route("getempcatedetails")]
        public ISM_PlannerReportsDTO getempcatedetails([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delg.getempcatedetails(data);
        }

        [Route("saveAddLop")]
        public ISM_PlannerReportsDTO saveAddLop([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.saveAddLop(data);
        }
        [Route("deactivate")]
        public ISM_PlannerReportsDTO deactivate([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }
        [Route("getonlodatdata/{id:int}")]
        public ISM_PlannerReportsDTO getonlodatdata(ISM_PlannerReportsDTO data)
        {
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getonlodatdata(data);
        }

        [Route("getconcalllopdetails")]
        public ISM_PlannerReportsDTO getconcalllopdetails([FromBody]ISM_PlannerReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delg.getconcalllopdetails(data);
        }
    }
}
