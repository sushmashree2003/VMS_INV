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
    public class ISM_EmployeeBlockReportFacade : Controller
    {

        public ISM_EmployeeBlockReportInterface _objinter;

        public ISM_EmployeeBlockReportFacade(ISM_EmployeeBlockReportInterface parameter)
        {
            _objinter = parameter;
        }
    
        [Route("getempblockReport")]
        public Task<ISM_BlockEmployeeDTO> getempblockReport([FromBody]ISM_BlockEmployeeDTO data)
        {
            return _objinter.getempblockReport(data);
        }
        //getempblockNewReport
        [Route("getempblockNewReport")]
        public Task<ISM_BlockEmployeeDTO> getempblockNewReport([FromBody]ISM_BlockEmployeeDTO data)
        {
            return _objinter.getempblockNewReport(data);
        }
    }
}
