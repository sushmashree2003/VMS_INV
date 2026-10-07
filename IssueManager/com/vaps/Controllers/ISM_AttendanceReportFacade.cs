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
    public class ISM_AttendanceReportFacade : Controller
    {
        public ISM_AttendanceReportInterface inter;
        public ISM_AttendanceReportFacade(ISM_AttendanceReportInterface parameter)
        {
            inter = parameter;
        }
        [Route("getdetails")]
        public Task<ISM_AttendanceReportsDTO> getdetails([FromBody] ISM_AttendanceReportsDTO data)
        {
            return inter.getdetails(data);
        }
        [Route("getplannerReport")]
        public Task<ISM_AttendanceReportsDTO> getplannerReport([FromBody]ISM_AttendanceReportsDTO data)
        {
            return inter.getplannerReport(data);
        }

    }
}
