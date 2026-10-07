using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class AttendanceApprovalReportController : Controller
    {
        AttendanceApprovalReportDelegate del = new AttendanceApprovalReportDelegate();

        [Route("getalldetails/{id:int}")]

        public AttendanceApprovalReportDTO getalldetails(int id)
        {
            AttendanceApprovalReportDTO data = new AttendanceApprovalReportDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return del.getdata(data);
        }

        [HttpPost]
        [Route("report")]
      public AttendanceApprovalReportDTO report([FromBody]AttendanceApprovalReportDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return del.report(data);
        }
    }
}
