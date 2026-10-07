using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class GeneralSendEmailController : Controller
    {
        GeneralSendEmailDelegate del = new GeneralSendEmailDelegate();

        [Route("sendsmsemail")]
        public SalesSMSEMAILDTO sendsmsemail([FromBody]SalesSMSEMAILDTO dto)
        {

            if (dto.MI_Id == 0)
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.sendsmsemail(dto);
        }
        //sendsmsemail
        [Route("sendsmsemailsales")]
        public SalesSMSEMAILDTO sendsmsemailsales([FromBody]SalesSMSEMAILDTO dto)
        {          
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.sendsmsemailsales(dto);
        }
        [Route("onload")]
        public SalesSMSEMAILDTO onload([FromBody]SalesSMSEMAILDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.onload(dto);
        }
        //onload
    }
}
