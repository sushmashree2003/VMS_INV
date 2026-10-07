using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace LeaveManagementServiceHub.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/LeaveoverallReportFacade")]
    public class LeaveoverallReportFacadeController : Controller
    {


        public LeaveOveallReportInerface _leave;

        public LeaveoverallReportFacadeController(LeaveOveallReportInerface _leavec)
        {
            _leave = _leavec;
        }
        [HttpPost]
        [Route("getleavereport")]
        public LeaveOverallReportDTO getleavereport([FromBody] LeaveOverallReportDTO data)
        {
            return _leave.getleavereport(data);
        }

        [Route("get_report")]
        public LeaveOverallReportDTO get_report([FromBody] LeaveOverallReportDTO data)
        {
            return _leave.get_report(data);
        }
        [Route("getleavetype")]
        public LeaveOverallReportDTO getleavetype([FromBody] LeaveOverallReportDTO data)
        {
            return _leave.getleavetype(data);
        }

        ////////MONTHENDREPORT////////////////////////////////////////////////////////////////////////////////

        [Route("getmonthendreport")]
        public LeaveOverallReportDTO getmonthendreport([FromBody] LeaveOverallReportDTO data)
        {
            return _leave.getmonthendreport(data);
        }
    }
}