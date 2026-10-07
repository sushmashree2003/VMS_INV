using HRMSServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class VMS_ReportFacadeController : Controller
    {
        public VMS_ReportInterface _ads;

        public VMS_ReportFacadeController(VMS_ReportInterface adstu)
        {
            _ads = adstu;
        }

        [Route("onloadgetdetails")]
        public VMS_ReportDTO onloadgetdetails([FromBody]VMS_ReportDTO dto)
        {
            return _ads.onloadgetdetails(dto);
        }

        [Route("report")]
        public VMS_ReportDTO report([FromBody]VMS_ReportDTO dto)
        {
            return _ads.report(dto);
        }

        [Route("get_Employee")]
        public VMS_ReportDTO get_Employee([FromBody]VMS_ReportDTO dto)
        {
            return _ads.get_Employee(dto);
        }
    }
    
}
