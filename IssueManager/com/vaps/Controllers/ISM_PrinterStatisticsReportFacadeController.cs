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
    public class ISM_PrinterStatisticsReportFacadeController : Controller
    {
        public ISM_PrinterStatisticsReportInterface _objinter;

        public ISM_PrinterStatisticsReportFacadeController(ISM_PrinterStatisticsReportInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_PrinterStatisticsDTO> getdetails([FromBody] ISM_PrinterStatisticsDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("get_report")]
        public Task<ISM_PrinterStatisticsDTO> get_report([FromBody]ISM_PrinterStatisticsDTO data)
        {
            return _objinter.get_report(data);
        }
        [Route("sendmail")]
        public Task<ISM_PrinterStatisticsDTO> sendmail([FromBody]ISM_PrinterStatisticsDTO data)
        {
            return _objinter.sendmail(data);
        }
    }
}
