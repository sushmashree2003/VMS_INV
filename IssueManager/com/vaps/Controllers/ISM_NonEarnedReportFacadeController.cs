using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_NonEarnedReportFacadeController : Controller
    {
        public ISM_NonEarnedReportInterface _objinter;

        public ISM_NonEarnedReportFacadeController(ISM_NonEarnedReportInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public ISM_NonEarnedReportDTO getalldata([FromBody]ISM_NonEarnedReportDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("Deptselectiondetails")]
        public ISM_NonEarnedReportDTO Deptselectiondetails([FromBody]ISM_NonEarnedReportDTO data)
        {
            return _objinter.Deptselectiondetails(data);
        }
        //Desgselectiondetails
        [Route("Desgselectiondetails")]
        public ISM_NonEarnedReportDTO Desgselectiondetails([FromBody]ISM_NonEarnedReportDTO data)
        {
            return _objinter.Desgselectiondetails(data);
        }
        [Route("getempblockReport")]
        public Task<ISM_NonEarnedReportDTO> getempblockReport([FromBody]ISM_NonEarnedReportDTO data)
        {
            return _objinter.getempblockReport(data);
        }
    }
}
