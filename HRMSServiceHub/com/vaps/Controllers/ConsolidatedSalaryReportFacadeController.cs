using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRMSServicesHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ConsolidatedSalaryReportFacadeController : Controller
    {
        // GET: api/values
        public ConsolidatedSalaryReportInterface _ads;

        public ConsolidatedSalaryReportFacadeController(ConsolidatedSalaryReportInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("onloadgetdetails")]
        public ConsolidatedSalaryReportDTO getinitialdata([FromBody]ConsolidatedSalaryReportDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        //FilterEmployeeData
        [Route("salaryreport")]
        public ConsolidatedSalaryReportDTO salaryreport([FromBody]ConsolidatedSalaryReportDTO dto)
        {
            return _ads.salaryreport(dto);
        }


    }
}
