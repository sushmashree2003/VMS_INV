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
    [Route("api/EmailSms_HistoryReportFacade")]
    public class EmailSms_HistoryReportFacadeController : Controller
    {
        public EmailSms_HistoryReportInterface _objinter;

        public EmailSms_HistoryReportFacadeController(EmailSms_HistoryReportInterface _obji)
        {
            _objinter = _obji;
        }
        [Route("getdetails")]
        public EmailSms_HistoryDTO getdetails([FromBody] EmailSms_HistoryDTO data)
        {
            return _objinter.getdetails(data);
        }
        //getmodule
        [Route("getmodule")]
        public EmailSms_HistoryDTO getmodule([FromBody] EmailSms_HistoryDTO data)
        {
            return _objinter.getmodule(data);
        }
        //
        [Route("getReport")]
        public EmailSms_HistoryDTO getReport([FromBody] EmailSms_HistoryDTO data)
        {
            return _objinter.getReport(data);
        }
    }
}