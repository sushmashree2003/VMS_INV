using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [Route("api/[controller]")]
    public class IEMonthEndSubmissionAndApprovalReportController : Controller
    {
        IEMonthEndSubmissionAndApprovalReportDelegate _del = new IEMonthEndSubmissionAndApprovalReportDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public IEMonthEndSubmissionAndApprovalReportDTO Onload(int n)
        {
            IEMonthEndSubmissionAndApprovalReportDTO data = new IEMonthEndSubmissionAndApprovalReportDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.Onload(data);
        }

        [HttpPost]
        [Route("getmonthreport")]
        public IEMonthEndSubmissionAndApprovalReportDTO getmonthreport([FromBody]IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.getmonthreport(data);
        }

        [HttpPost]
        [Route("DocView")]
        public IEMonthEndSubmissionAndApprovalReportDTO DocView([FromBody]IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.DocView(data);
        }

        [HttpPost]
        [Route("getIEDetails")]
        public IEMonthEndSubmissionAndApprovalReportDTO getIEDetails([FromBody]IEMonthEndSubmissionAndApprovalReportDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.getIEDetails(data);
        }

    }
}