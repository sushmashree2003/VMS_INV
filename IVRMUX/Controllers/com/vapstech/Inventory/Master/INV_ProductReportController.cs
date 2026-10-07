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
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class INV_ProductReportController : Controller
    {

        INV_ProductReportDelegate inv = new INV_ProductReportDelegate();
     

    
        [Route("getalldetails")]
        public INV_Master_ProductDTO getalldetails([FromBody] INV_Master_ProductDTO data)
        {
           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return inv.getalldetails(data);

        }

        [Route("radiobtndata")]
        public INV_Master_ProductDTO radiobtndata([FromBody]INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return inv.radiobtndata(data);

        }

        

        [Route("getdata")]
        public INV_Master_ProductDTO getdata([FromBody]INV_Master_ProductDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return inv.getdata(data);

        }


    }
}
