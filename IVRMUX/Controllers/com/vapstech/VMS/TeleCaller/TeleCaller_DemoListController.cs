using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TeleCaller;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class TeleCaller_DemoListController : Controller
    {
        TeleCaller_DemoLlistDelegate del = new TeleCaller_DemoLlistDelegate();
        
        [Route("TeleCaller_List")]
        public TeleCallerDTO TeleCaller_List([FromBody]TeleCallerDTO dto)
        {

            if (dto.MI_Id == 0)
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
          
            return del.TeleCaller_List(dto);
        }
        [Route("UpdateTeleCaller_List")]
        public TeleCallerDTO UpdateTeleCaller_List([FromBody]TeleCallerDTO dto)
        {

            if (dto.MI_Id == 0)
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            dto.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.UpdateTeleCaller_List(dto);
        }


    }
}
