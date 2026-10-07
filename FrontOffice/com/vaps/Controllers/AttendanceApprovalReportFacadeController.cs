using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class AttendanceApprovalReportFacadeController : Controller
    {
        public AttendanceApprovalReportInterface _org;

        public AttendanceApprovalReportFacadeController(AttendanceApprovalReportInterface maspag)
        {
            _org = maspag;
        }

       
        [Route("getalldetails")]
        public AttendanceApprovalReportDTO getalldetails([FromBody] AttendanceApprovalReportDTO data)
        {
            return _org.getdata(data);
        }

     
        [Route("report")]
        public AttendanceApprovalReportDTO report([FromBody] AttendanceApprovalReportDTO data)
        {
            return _org.report(data);
        }
    }
}
