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
    [Route("api/TADAApply_Payment")]
    public class TADAApply_PaymentController : Controller
    {
        TADAApply_PaymentDelegete _delegate = new TADAApply_PaymentDelegete();
        // 
        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADAApply_PaymentDTO getalldata(int id)
        {
            TADAApply_PaymentDTO dto = new TADAApply_PaymentDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }
        //savedta
        [HttpPost]
        [Route("savedta")]
        public TADAApply_PaymentDTO savedta([FromBody]TADAApply_PaymentDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(dto);


        }
        
        [Route("deactive")]
        public TADAApply_PaymentDTO deactive([FromBody]TADAApply_PaymentDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(dto);
        }
        //edittt
        [Route("edittt")]
        public TADAApply_PaymentDTO edittt([FromBody]TADAApply_PaymentDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edittt(dto);
        }
        [Route("Viewdetails")]
        public TADAApply_PaymentDTO Viewdetails([FromBody]TADAApply_PaymentDTO dto)
        {

            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Viewdetails(dto);
        }
        //
    }
}