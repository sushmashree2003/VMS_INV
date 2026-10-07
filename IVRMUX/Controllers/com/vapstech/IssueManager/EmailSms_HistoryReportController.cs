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
    [Route("api/EmailSms_HistoryReport")]
    public class EmailSms_HistoryReportController : Controller
    {
        EmailSms_HistoryDelegate _delg = new EmailSms_HistoryDelegate();       
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public EmailSms_HistoryDTO getdetails(EmailSms_HistoryDTO data)
        {          
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getdetails(data);
        }
        //getmodule
        [HttpPost]
        [Route("getmodule")]
        public EmailSms_HistoryDTO getPlannerlist([FromBody] EmailSms_HistoryDTO data)
       {
        
           return _delg.getmodule(data);
       }
        //getReport
        [HttpPost]
        [Route("getReport")]
        public EmailSms_HistoryDTO getReport([FromBody] EmailSms_HistoryDTO data)
        {

            return _delg.getReport(data);
        }
    }
}