using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.Inventory
{
    [Route("api/[controller]")]
    public class INV_VendorCategoryMasterController : Controller
    {
        INV_VendorCategoryMasterDelegate _delegate = new INV_VendorCategoryMasterDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public INV_VendorCategoryMasterDTO getloaddata(int id)
        {
            INV_VendorCategoryMasterDTO data = new INV_VendorCategoryMasterDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }

        [Route("savedetails")]
        public INV_VendorCategoryMasterDTO savedetails([FromBody] INV_VendorCategoryMasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public INV_VendorCategoryMasterDTO deactive([FromBody] INV_VendorCategoryMasterDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
    }
}
