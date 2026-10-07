using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class OrderApprovalController : Controller
    {
        OrderApprovalDelegate _delegate = new OrderApprovalDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public OrderApprovalDTO getloaddata(int id)
        {
            OrderApprovalDTO data = new OrderApprovalDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public OrderApprovalDTO savedetails([FromBody] OrderApprovalDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [Route("Rejectorder")]
        public OrderApprovalDTO Rejectorder([FromBody] OrderApprovalDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Rejectorder(data);
        }

        [Route("getpidetails")]
        public OrderApprovalDTO getpidetails([FromBody] OrderApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getpidetails(data);
        }

        [Route("getprDetail")]
        public OrderApprovalDTO getprDetail([FromBody] OrderApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getprDetail(data);
        }

        [Route("deactiveM")]
        public OrderApprovalDTO deactiveM([FromBody] OrderApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactiveM(data);
        }

        [Route("deactive")]
        public OrderApprovalDTO deactive([FromBody] OrderApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }

        [Route("get_details")]
        public OrderApprovalDTO get_details([FromBody] OrderApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.get_details(data);
        }

        [Route("edit")]
        public OrderApprovalDTO edit([FromBody] OrderApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edit(data);
        }

        [Route("rejectallreq")]
        public OrderApprovalDTO rejectallreq([FromBody] OrderApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.rejectallreq(data);
        }

        [Route("viewsavedteple")]
        public OrderApprovalDTO viewsavedteple([FromBody] OrderApprovalDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.viewsavedteple(data);
        }

        [Route("viewtemplate")]
        public OrderApprovalDTO viewtemplate([FromBody] OrderApprovalDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.viewtemplate(data);
        }
        

        [Route("OnClickIndividualViewComments")]
        public OrderApprovalDTO OnClickIndividualViewComments([FromBody] OrderApprovalDTO data)
        {            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.OnClickIndividualViewComments(data);
        }
    }
}
