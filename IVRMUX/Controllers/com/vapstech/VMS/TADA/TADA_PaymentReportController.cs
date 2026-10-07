using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IVRMUX.Controllers.com.vapstech.VMS.TADA
{
    [Produces("application/json")]
    [Route("api/TADA_PaymentReport")]
    public class TADA_PaymentReportController : Controller
    {
        TADA_PaymentReportDelegete _delegate = new TADA_PaymentReportDelegete();

        // 
        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADA_PaymentReportDTO getalldata(int id)
        {
            TADA_PaymentReportDTO dto = new TADA_PaymentReportDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }
        //savedta
        [HttpPost]
        [Route("GetReport")]
        public TADA_PaymentReportDTO GetReport([FromBody]TADA_PaymentReportDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.GetReport(dto);


        }

    }
}