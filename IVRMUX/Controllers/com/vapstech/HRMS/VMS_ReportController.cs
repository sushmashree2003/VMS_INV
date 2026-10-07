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
    public class VMS_ReportController : Controller
    {
        VMS_ReportDelegate del = new VMS_ReportDelegate();

        [Route("onloadgetdetails/{id:int}")]
        public VMS_ReportDTO onloadgetdetails(int id)
        {
            VMS_ReportDTO dto = new VMS_ReportDTO();

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.onloadgetdetails(dto);
        }


        [HttpPost]
        [Route("report")]
        public VMS_ReportDTO report([FromBody]VMS_ReportDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.report(dto);
        }


        [HttpPost]
        [Route("get_Employee")]
        public VMS_ReportDTO get_Employee([FromBody]VMS_ReportDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_Employee(dto);
        }
    }
}
