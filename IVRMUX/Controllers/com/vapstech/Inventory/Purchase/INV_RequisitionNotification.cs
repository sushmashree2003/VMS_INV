using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory.Purchase;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Inventory.Purchase
{
    [Route("api/[controller]")]
    public class INV_RequisitionNotification : Controller
    {
        INV_RequisitionNotificationDelegate _delegate = new INV_RequisitionNotificationDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_RequisitionNotificationDTO getloaddata(int id)
        {
            INV_RequisitionNotificationDTO data = new INV_RequisitionNotificationDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }
        [Route("get_prdetails")]
        public INV_RequisitionNotificationDTO get_prdetails([FromBody] INV_RequisitionNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_prdetails(data);
        }

        [Route("getitemDetail")]
        public INV_RequisitionNotificationDTO getitemDetail([FromBody] INV_RequisitionNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemDetail(data);
        }

        [Route("savedetails")]
        public INV_RequisitionNotificationDTO savedetails([FromBody] INV_RequisitionNotificationDTO data)
        {
          
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.savedetails(data);
        }
        [Route("edit")]
        public INV_RequisitionNotificationDTO edit([FromBody] INV_RequisitionNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.edit(data);
        }

        [Route("deactive")]
        public INV_RequisitionNotificationDTO deactive([FromBody] INV_RequisitionNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

        [Route("deactiveM")]
        public INV_RequisitionNotificationDTO deactiveM([FromBody] INV_RequisitionNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactiveM(data);
        }

        [Route("GetOnchangeData")]
        public INV_RequisitionNotificationDTO GetOnchangeData([FromBody] INV_RequisitionNotificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.GetOnchangeData(data);
        }

    }
}
