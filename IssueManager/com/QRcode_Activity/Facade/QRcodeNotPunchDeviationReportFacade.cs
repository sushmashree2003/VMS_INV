using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.QRcode_Activity.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.QRcode_Activity.Facade
{
    [Route("api/[controller]")]
    public class QRcodeNotPunchDeviationReportFacade : Controller
    {
        // GET: api/values
        QRcodeNotPunchDeviationReportInterface _Inv;

        public QRcodeNotPunchDeviationReportFacade(QRcodeNotPunchDeviationReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public QRcodeNotPunchDeviationReportDTO getloaddata([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {
            return _Inv.getloaddata(data);
        }


        [Route("get_Floors")]
        public QRcodeNotPunchDeviationReportDTO get_Floors([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {
            return _Inv.get_Floors(data);
        }

        [Route("get_Areas")]
        public QRcodeNotPunchDeviationReportDTO get_Areas([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {
            return _Inv.get_Areas(data);
        }

        [Route("getEmployeeName")]
        public QRcodeNotPunchDeviationReportDTO getEmployeeName([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {
            return _Inv.getEmployeeName(data);
        }


        [HttpPost]

        [Route("QRcodedeviation")]
        public QRcodeNotPunchDeviationReportDTO QRcodedeviation([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {

            return _Inv.QRcodedeviation(data);
        }

        [HttpPost]

        [Route("SendMail")]
        public QRcodeNotPunchDeviationReportDTO SendMail([FromBody] QRcodeNotPunchDeviationReportDTO data)
        {

            return _Inv.SendMail(data);
        }

    }
}
