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
    public class QRcodeDeviationReportFacade : Controller
    {
        // GET: api/values
        QRcodeDeviationReportInterface _Inv;

        public QRcodeDeviationReportFacade(QRcodeDeviationReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public QRcodeDeviationReportDTO getloaddata([FromBody] QRcodeDeviationReportDTO data)
        {
            return _Inv.getloaddata(data);
        }

        [Route("getEmployeeName")]
        public QRcodeDeviationReportDTO getEmployeeName([FromBody] QRcodeDeviationReportDTO data)
        {
            return _Inv.getEmployeeName(data);
        }


        [HttpPost]

        [Route("QRcodedeviation")]
        public QRcodeDeviationReportDTO QRcodedeviation([FromBody] QRcodeDeviationReportDTO data)
        {

            return _Inv.QRcodedeviation(data);
        }

    }
}
