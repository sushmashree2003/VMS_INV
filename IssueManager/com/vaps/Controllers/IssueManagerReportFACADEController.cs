using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/IssueManagerReportFACADE")]
    public class IssueManagerReportFACADEController : Controller
    {
        public IssueManagerReportInterface _objinter;

        public IssueManagerReportFACADEController(IssueManagerReportInterface _obji)
        {
            _objinter = _obji;
        }
        [Route("getdetails")]
        public IssueManagerReportDTO getdetails([FromBody] IssueManagerReportDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("getReport")]
        public IssueManagerReportDTO getReport([FromBody] IssueManagerReportDTO data)
        {
            return _objinter.getReport(data);
        }
    }
}