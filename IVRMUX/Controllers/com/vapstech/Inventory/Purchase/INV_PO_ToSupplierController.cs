
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
    public class INV_PO_ToSupplierController : Controller
    {
        INV_PO_ToSupplierDelegate _delegate = new INV_PO_ToSupplierDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public INV_PO_ToSupplierDTO getloaddata(int id)
        {
            INV_PO_ToSupplierDTO data = new INV_PO_ToSupplierDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getpiDetail")]
        public INV_PO_ToSupplierDTO getpiDetail([FromBody] INV_PO_ToSupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getpiDetail(data);
        }

        [Route("savedetails")]
        public INV_PO_ToSupplierDTO savedetails([FromBody] INV_PO_ToSupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public INV_PO_ToSupplierDTO deactive([FromBody] INV_PO_ToSupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

        [Route("viewtemplatedetails")]
        public INV_PO_ToSupplierDTO viewtemplatedetails([FromBody] INV_PO_ToSupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.viewtemplatedetails(data);
        }

        [Route("sendmail")]
        public INV_PO_ToSupplierDTO sendmail([FromBody] INV_PO_ToSupplierDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.sendmail(data);
        }








    }
}
