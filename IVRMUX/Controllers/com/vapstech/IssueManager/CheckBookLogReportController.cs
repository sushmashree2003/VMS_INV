
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
    public class CheckBookLogReportController : Controller
    {
        CheckBookLogReportDelegate del = new CheckBookLogReportDelegate();

        [Route("loaddata/{id:int}")]
        public CheckBookLogReportDTO loaddata(int id)
        {
            CheckBookLogReportDTO data = new CheckBookLogReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }

        [Route("onchange")]
        public CheckBookLogReportDTO onchange([FromBody]CheckBookLogReportDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.onchange(data);

        }

        [Route("Report")]
        public CheckBookLogReportDTO Report([FromBody]CheckBookLogReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.Report(data);

        }


    }
}

