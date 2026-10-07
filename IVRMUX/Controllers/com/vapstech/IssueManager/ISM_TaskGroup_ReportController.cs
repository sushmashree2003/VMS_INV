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
    public class ISM_TaskGroup_ReportController : Controller
    {
        public ISM_TaskGroup_ReportDelegate del = new ISM_TaskGroup_ReportDelegate();
        // GET: api/<controller>
        [Route("getdetails/{id:int}")]
        public ISM_TaskGroup_ReportDTO getdetails(int id)
        {
            ISM_TaskGroup_ReportDTO data = new ISM_TaskGroup_ReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdetails(data);
        }

        //[Route("getdepartments")]
        //public ISM_TaskGroup_ReportDTO getdepartments([FromBody]ISM_TaskGroup_ReportDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));            
        //    return del.getdepartments(data);
        //}
        [Route("getplannerReport")]
        public ISM_TaskGroup_ReportDTO getplannerReport([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getplannerReport(data);
        }
        [Route("getReport")]
        public ISM_TaskGroup_ReportDTO getReport([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getReport(data);
        }

        /* CATEGORY GROUP */
        [Route("getcategorydetails")]
        public ISM_TaskGroup_ReportDTO getcategorydetails([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getcategorydetails(data);
        }
     

        [Route("onchangedept")]
        public ISM_TaskGroup_ReportDTO onchangedept([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onchangedept(data);
        }
        [Route("categorygroupReport")]
        public ISM_TaskGroup_ReportDTO categorygroupReport([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.categorygroupReport(data);
        }
        //SaveFeedBack
        [Route("SaveFeedBack")]
        public ISM_TaskGroup_ReportDTO SaveFeedBack([FromBody]ISM_TaskGroup_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.SaveFeedBack(data);
        }
    }
}
