using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]

    public class ISM_PrinterStatisticsFacade : Controller
    {
        public ISM_PrinterStatisticsInterface _objinter;

        public ISM_PrinterStatisticsFacade(ISM_PrinterStatisticsInterface parameter)
        {
            _objinter = parameter;
        }
       
        [Route("getdetails")]
        public Task<ISM_PrinterStatisticsDTO> getdetails([FromBody] ISM_PrinterStatisticsDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("savedata")]
        public ISM_PrinterStatisticsDTO savedata([FromBody]ISM_PrinterStatisticsDTO data)
        {
            return _objinter.savedata(data);
        }

        [Route("deactive")]
        public ISM_PrinterStatisticsDTO deactive([FromBody]ISM_PrinterStatisticsDTO data)
        {
            return _objinter.deactive(data);
        }
        [Route("gettaskAttachment")]
        public ISM_PrinterStatisticsDTO gettaskAttachment([FromBody]ISM_PrinterStatisticsDTO data)
        {
            return _objinter.gettaskAttachment(data);
        }

        [Route("PrinterEmail")]
        public ISM_PrinterStatisticsDTO DuedateRemainder([FromBody] ISM_PrinterStatisticsDTO data)
        {
            return _objinter.PrinterEmail(data);
        }
        [Route("deptchange")]
        public ISM_PrinterStatisticsDTO deptchange([FromBody]ISM_PrinterStatisticsDTO data)
        {
            return _objinter.deptchange(data);
        }

    }
}
