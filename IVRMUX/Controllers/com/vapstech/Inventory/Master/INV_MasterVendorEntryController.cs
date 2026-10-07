using IVRMUX.Delegates.com.vapstech.Inventory.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Master
{
    [Route("api/[controller]")]
    public class INV_MasterVendorEntryController : Controller
    {
        INV_MasterVendorEntryDelegate _delegate = new INV_MasterVendorEntryDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public INV_MasterVendorEntryDTO getloaddata(int id)
        {
            INV_MasterVendorEntryDTO data = new INV_MasterVendorEntryDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public INV_MasterVendorEntryDTO savedetails([FromBody] INV_MasterVendorEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [HttpPost]
        [Route("deactive")]
        public INV_MasterVendorEntryDTO deactive([FromBody] INV_MasterVendorEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }

        [HttpPost]
        [Route("CategoryDropDown")]
        public INV_MasterVendorEntryDTO CategoryDropDown([FromBody] INV_MasterVendorEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.CategoryDropDown(data);
        }
    }
}