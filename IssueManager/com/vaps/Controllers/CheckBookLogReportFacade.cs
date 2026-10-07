   using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CheckBookLogReportFacade : Controller
    {
        public CheckBookLogReportInterface inter;
        public CheckBookLogReportFacade(CheckBookLogReportInterface s)
        {
            inter = s;
        }

        [Route("loaddata")]
        public CheckBookLogReportDTO loaddata([FromBody] CheckBookLogReportDTO data)
        {
            return inter.loaddata(data);
        }

        [Route("onchange")]
        public CheckBookLogReportDTO onchange([FromBody] CheckBookLogReportDTO data)
        {
            return inter.onchange(data);
        }

        [Route("Report")]
        public CheckBookLogReportDTO Report([FromBody] CheckBookLogReportDTO data)
        {
            return inter.Report(data);
        }
    }
}
