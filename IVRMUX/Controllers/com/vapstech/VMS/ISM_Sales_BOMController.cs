using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class ISM_Sales_BOMController : Controller
    {
        ISM_Sales_BOMDelegate LTM = new ISM_Sales_BOMDelegate();
    

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public ISM_Sales_BOMDTO getloaddata(int id)
        {
            ISM_Sales_BOMDTO dto = new ISM_Sales_BOMDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.getloaddata(dto);
        }


        [HttpPost]
        [Route("savedetails")]
        public ISM_Sales_BOMDTO savedetails([FromBody] ISM_Sales_BOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.savedetails(data);
        }
        [HttpPost]
        [Route("saveiteamsdetails")]
        public ISM_Sales_BOMDTO saveiteamsdetails([FromBody] ISM_Sales_BOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.saveiteamsdetails(data);
        }
        [HttpPost]
        [Route("deactive")]
        public ISM_Sales_BOMDTO deactive([FromBody] ISM_Sales_BOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.deactive(data);
        }

        [HttpPost]
        [Route("saveBOMdetails")]
        public ISM_Sales_BOMDTO saveBOMdetails([FromBody] ISM_Sales_BOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.saveBOMdetails(data);
        }


        [HttpPost]
        [Route("viewservicedetails")]
        public ISM_Sales_BOMDTO viewservicedetails([FromBody] ISM_Sales_BOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.viewservicedetails(data);
        }

        [HttpPost]
        [Route("Sales_BOM_edit")]
        public ISM_Sales_BOMDTO Sales_BOM_edit([FromBody] ISM_Sales_BOMDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.Sales_BOM_edit(data);
        }


    }
}
