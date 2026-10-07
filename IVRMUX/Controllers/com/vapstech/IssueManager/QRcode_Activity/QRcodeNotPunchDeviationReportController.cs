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
    public class QRcodeNotPunchDeviationReportController : Controller
    {

        QRcodeNotPunchDeviationReportDelegate _delegate = new QRcodeNotPunchDeviationReportDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public QRcodeNotPunchDeviationReportDTO getloaddata(int id)
        {
            QRcodeNotPunchDeviationReportDTO data = new QRcodeNotPunchDeviationReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }


        //post
        [HttpPost]
        [Route("get_Floors")]
        public QRcodeNotPunchDeviationReportDTO get_Floors([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_Floors(data);
        }


        //post
        [HttpPost]
        [Route("get_Areas")]
        public QRcodeNotPunchDeviationReportDTO get_Areas([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_Areas(data);
        }

        //post
        [HttpPost]
        [Route("getEmployeeName")]
        public QRcodeNotPunchDeviationReportDTO getEmployeeName([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getEmployeeName(data);
        }


        [HttpPost]
        [Route("QRcodedeviation")]
        public QRcodeNotPunchDeviationReportDTO QRcodedeviation([FromBody] QRcodeNotPunchDeviationReportDTO data)

        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.QRcodedeviation(data);
        }
        [HttpPost]
        [Route("SendMail")]
        public QRcodeNotPunchDeviationReportDTO SendMail([FromBody] QRcodeNotPunchDeviationReportDTO data)

        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.SendMail(data);
        }


    }
}