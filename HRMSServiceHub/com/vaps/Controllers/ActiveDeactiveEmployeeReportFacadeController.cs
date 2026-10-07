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
        public class ActiveDeactiveEmployeeReportFacadeController : Controller
        {
        public ActiveDeactiveEmployeeReportInterface _ads;

        public ActiveDeactiveEmployeeReportFacadeController(ActiveDeactiveEmployeeReportInterface adstu)
        {
            _ads = adstu;

        }

        [Route("getalldetails")]
        public ActiveDeactiveEmployeeReportDTO getalldetails([FromBody]ActiveDeactiveEmployeeReportDTO data)
        {
            return _ads.getalldetails(data);
        }

        [Route("report")]
        public ActiveDeactiveEmployeeReportDTO report([FromBody]ActiveDeactiveEmployeeReportDTO data)
        {
            return _ads.report(data);
        }

    }

 }
