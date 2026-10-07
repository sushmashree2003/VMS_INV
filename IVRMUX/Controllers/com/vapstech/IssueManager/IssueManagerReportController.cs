using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Produces("application/json")]
    [Route("api/IssueManagerReport")]
    public class IssueManagerReportController : Controller
    {
        IssueManagerReportDelegate _delg = new IssueManagerReportDelegate();
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public IssueManagerReportDTO getdetails(IssueManagerReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getdetails(data);
        }
        [HttpPost]
        [Route("getReport")]
        public IssueManagerReportDTO getReport([FromBody] IssueManagerReportDTO data)
        {
            return _delg.getReport(data);
        }
    }
}