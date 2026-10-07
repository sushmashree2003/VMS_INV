using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using AssetTrackingServiceHub.com.vaps.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class DCReportFacadeController : Controller
    {
        // GET: api/values
        DCReportInterface _AT;
        public DCReportFacadeController(DCReportInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public Task<DCReportDTO> getloaddata([FromBody] DCReportDTO data)
        {
            return _AT.getloaddata(data);
        }


        [Route("getreport")]
        public Task<DCReportDTO> getreport([FromBody] DCReportDTO data)
        {
            return _AT.getreport(data);
        }





    }
}
