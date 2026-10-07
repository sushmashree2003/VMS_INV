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
    public class INV_VenderApprovalController : Controller
    {
        INV_VenderApprovalDelegate _delegate = new INV_VenderApprovalDelegate();

        [HttpPost]
        [Route("getloaddataOne")]
        public async Task<INV_MasterVendorEntryDTO> getloaddataOne([FromBody] INV_MasterVendorEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return await _delegate.getloaddataOne(data);
        }

        [HttpPost]
        [Route("getloaddataTwo")]
        public async Task<INV_MasterVendorEntryDTO> getloaddataTwo([FromBody] INV_MasterVendorEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return await _delegate.getloaddataTwo(data);
        }

        [HttpPost]
        [Route("UpdateSP")]
        public async Task<INV_MasterVendorEntryDTO> UpdateSP([FromBody] INV_MasterVendorEntryDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return await _delegate.UpdateSP(data);
        }
    }
}
