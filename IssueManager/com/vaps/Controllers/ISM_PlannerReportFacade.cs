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
    public class ISM_PlannerReportFacade : Controller
    {
        public ISM_PlannerReportInterface inter;
        public ISM_PlannerReportFacade(ISM_PlannerReportInterface parameter)
        {
            inter = parameter;
        }
        [Route("getdetails")]
        public Task<ISM_PlannerReportsDTO> getdetails([FromBody] ISM_PlannerReportsDTO data)
        {
            return inter.getdetails(data);
        }
        [Route("getplannerReport")]
        public Task<ISM_PlannerReportsDTO> getplannerReport([FromBody]ISM_PlannerReportsDTO data)
        {
            return inter.getplannerReport(data);
        }

    }
}
