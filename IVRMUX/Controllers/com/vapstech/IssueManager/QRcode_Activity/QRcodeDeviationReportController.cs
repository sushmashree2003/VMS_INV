using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.QRcode_Activity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;

namespace IVRMUX.Controllers.com.vapstech.IssueManager.QRcode_Activity
{
    [Route("api/[controller]")]
    public class QRcodeDeviationReportController : Controller
    {

        QRcodeDeviationReportDelegate _delegate = new QRcodeDeviationReportDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public QRcodeDeviationReportDTO getloaddata(int id)
        {
            QRcodeDeviationReportDTO data = new QRcodeDeviationReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        //post
        [HttpPost]
        [Route("getEmployeeName")]
        public QRcodeDeviationReportDTO getEmployeeName([FromBody] QRcodeDeviationReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getEmployeeName(data);
        }


        [HttpPost]
        [Route("QRcodedeviation")]
        public QRcodeDeviationReportDTO QRcodedeviation([FromBody] QRcodeDeviationReportDTO data)

        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.QRcodedeviation(data);
        }
        
       

    }
}