using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class TrackerReportFacadeController : Controller
    {
        public TrackerReportInterface _ads;

        public TrackerReportFacadeController(TrackerReportInterface adstu)
        {
            _ads = adstu;
        }

        [Route("findstatistics")]
        public Task<TrackerReportDTO> findstatistics([FromBody]TrackerReportDTO dto)
        {
            return _ads.findstatisticsAsync(dto);
        }
    }
}
