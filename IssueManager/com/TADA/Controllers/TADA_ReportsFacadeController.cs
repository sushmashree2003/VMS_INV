using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IssueManager.com.TADA.Controllers
{
    [Produces("application/json")]
    [Route("api/TADA_ReportsFacade")]
    public class TADA_ReportsFacadeController : Controller
    {
        public TADA_ReportsInterface _objinter;

        public TADA_ReportsFacadeController(TADA_ReportsInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADA_ApplyReportsDTO getalldata([FromBody]TADA_ApplyReportsDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("Deptselectiondetails")]
        public TADA_ApplyReportsDTO Deptselectiondetails([FromBody]TADA_ApplyReportsDTO data)
        {
            return _objinter.Deptselectiondetails(data);
        }
        //Desgselectiondetails
        [Route("Desgselectiondetails")]
        public TADA_ApplyReportsDTO Desgselectiondetails([FromBody]TADA_ApplyReportsDTO data)
        {
            return _objinter.Desgselectiondetails(data);
        }
        //GetReport
        [Route("GetReport")]
        public TADA_ApplyReportsDTO GetReport([FromBody]TADA_ApplyReportsDTO data)
        {
            return _objinter.GetReport(data);
        }
        //Viewdetails
        [Route("Viewdetails")]
        public TADA_ApplyReportsDTO Viewdetails([FromBody]TADA_ApplyReportsDTO data)
        {
            return _objinter.Viewdetails(data);
        }
    }
}