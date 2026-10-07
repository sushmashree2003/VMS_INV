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
    public class INV_VendorSignOffController : Controller
    {
        INV_VendorSignOffDelegate _delegate = new INV_VendorSignOffDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public INV_VendorSignOffDTO getloaddata(int id)
        {
            INV_VendorSignOffDTO data = new INV_VendorSignOffDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getloaddata(data);
        }

        [HttpPost]
        [Route("savedetails")]
        public INV_VendorSignOffDTO savedetails([FromBody] INV_VendorSignOffDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }

        [HttpPost]
        [Route("deactive")]
        public INV_VendorSignOffDTO deactive([FromBody] INV_VendorSignOffDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
    }
}
