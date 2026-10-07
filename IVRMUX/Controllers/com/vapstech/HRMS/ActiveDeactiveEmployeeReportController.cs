using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.HRMS
{

    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ActiveDeactiveEmployeeReportController : Controller
    {
        ActiveDeactiveEmployeeReportDelgate del = new ActiveDeactiveEmployeeReportDelgate();

        [Route("getalldetails/{id:int}")]
        public ActiveDeactiveEmployeeReportDTO getalldetails(int id)
        {
            ActiveDeactiveEmployeeReportDTO data = new ActiveDeactiveEmployeeReportDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getalldetails(data);
        }


        [HttpPost]
        [Route("report")]
        public ActiveDeactiveEmployeeReportDTO report([FromBody]ActiveDeactiveEmployeeReportDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.report(data);
        }

    }
}
