using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.TADA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CheckBookEntryReportFacade : Controller
    {
        public CheckBookEntryReportInterface inter;
        public CheckBookEntryReportFacade(CheckBookEntryReportInterface s)
        {
            inter = s;
        }
        [Route("getloaddata")]
        public CheckBookEntryReportDTO getloaddata([FromBody] CheckBookEntryReportDTO data)
        {
            return inter.getloaddata(data);
        }
        [Route("onreport")]
        public CheckBookEntryReportDTO savedata([FromBody] CheckBookEntryReportDTO data)
        {
            return inter.onreport(data);
        }
     
    }
}
