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
    [Route("api/TADA_PaymentReportFacade")]
    public class TADA_PaymentReportFacadeController : Controller
    {
        public TADA_PaymentReportInterface _objinter;

        public TADA_PaymentReportFacadeController(TADA_PaymentReportInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADA_PaymentReportDTO getalldata([FromBody]TADA_PaymentReportDTO data)
        {
            return _objinter.getalldata(data);
        }
        //savedta
        [Route("GetReport")]
        public TADA_PaymentReportDTO savedta([FromBody]TADA_PaymentReportDTO data)
        {
            return _objinter.GetReport(data);
        }
       

    }
}