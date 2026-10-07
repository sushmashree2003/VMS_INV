using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FMS.Com.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FMS.Com.Facade
{
    [Route("api/[controller]")]
    public class FMSApprovalReportFacade : Controller
    {
        public FMSApprovalReportInterface _interface;

        public FMSApprovalReportFacade(FMSApprovalReportInterface _inter)
        {
            _interface = _inter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("loaddata")]
        public FMSApprovalReportDTO loaddata([FromBody] FMSApprovalReportDTO data)
        {
            return _interface.loaddata(data);
        }

        [Route("report")]
        public FMSApprovalReportDTO report([FromBody] FMSApprovalReportDTO data)
        {
            return _interface.report(data);
        }
    }
}
