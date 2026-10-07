using System;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOffice.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class FrontOfficeLogReportFacadeController : Controller
    {
        public FrontOfficeLogReportInterface _objInter;
        public FrontOfficeLogReportFacadeController(FrontOfficeLogReportInterface data)
        {
            _objInter = data;
        }

        [Route("Savedata")]
        public FrontOfficeLogReportDTO Savedata([FromBody] FrontOfficeLogReportDTO data)
        {
            return _objInter.Savedata(data);
        }
              
    }
}
