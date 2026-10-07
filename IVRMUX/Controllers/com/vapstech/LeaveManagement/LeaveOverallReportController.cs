using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace IVRMUX.Controllers.com.vapstech.LeaveManagement
{
    [Produces("application/json")]
    [Route("api/LeaveOverallReport")]
    public class LeaveOverallReportController : Controller
    {
        LeaveOverallReportDelegate ltmd = new LeaveOverallReportDelegate();
       [HttpGet]
        [Route("getleavereport/{id:int}")]
        public LeaveOverallReportDTO getleavereport(int id)
        {
            LeaveOverallReportDTO data = new LeaveOverallReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.getleavereport(data);

            //var response = client.PostAsync("api/LeaveoverallReportFacade/get_report/", byteContent).Result;
        }

        [HttpPost]
        [Route("get_report")]
        public LeaveOverallReportDTO get_report([FromBody] LeaveOverallReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.get_report(data);
        }
        [Route("getleavetype")]
        public LeaveOverallReportDTO getleavetype([FromBody] LeaveOverallReportDTO data)
        {
            return ltmd.getleavetype(data);
        }

        ////////MONTHENDREPORT////////////////////////////////////////////////////////////////////////////////
        [HttpPost]
        [Route("getmonthendreport")]
        public LeaveOverallReportDTO getmonthendreport([FromBody] LeaveOverallReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return ltmd.getmonthendreport(data);
        }

    }
}