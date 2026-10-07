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
    [Route("api/TADA_AdavanceApply_Payment")]
    public class TADA_AdavanceApply_PaymentController : Controller
    {
        TADA_AdavanceApply_PaymentDelegate _delegate = new TADA_AdavanceApply_PaymentDelegate();

        // 
        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADA_AdavanceApply_PaymentDTO getalldata(int id)
        {
            TADA_AdavanceApply_PaymentDTO dto = new TADA_AdavanceApply_PaymentDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }
        //getOnload
        [HttpGet]
        [Route("getOnload/{id:int}")]
        public TADA_AdavanceApply_PaymentDTO getOnload(int id)
        {
            TADA_AdavanceApply_PaymentDTO dto = new TADA_AdavanceApply_PaymentDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getOnload(dto);


        }
        //savedta
        [HttpPost]
        [Route("savedta")]
        public TADA_AdavanceApply_PaymentDTO savedta([FromBody]TADA_AdavanceApply_PaymentDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(dto);


        }
        //savePayment
        [Route("savePayment")]
        public TADA_AdavanceApply_PaymentDTO savePayment([FromBody]TADA_AdavanceApply_PaymentDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savePayment(dto);


        }
        [Route("deactive")]
        public TADA_AdavanceApply_PaymentDTO deactive([FromBody]TADA_AdavanceApply_PaymentDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(dto);


        }
        //edittt
        [Route("edittt")]
        public TADA_AdavanceApply_PaymentDTO edittt([FromBody]TADA_AdavanceApply_PaymentDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edittt(dto);


        }
        [Route("Viewdetails")]
        public TADA_AdavanceApply_PaymentDTO Viewdetails([FromBody]TADA_AdavanceApply_PaymentDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Viewdetails(dto);


        }
        //Viewdetails
    }
}