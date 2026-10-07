using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IVRMUX.Controllers.com.vapstech.VMS.TADA
{
    [Produces("application/json")]
    [Route("api/TADA_Advance_Apply")]
    public class TADA_Advance_ApplyController : Controller
    {
        TADA_Advance_ApplyDelegate _delegate = new TADA_Advance_ApplyDelegate();
        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADA_Advance_ApplyDTO getalldata(int id)
        {
            TADA_Advance_ApplyDTO dto = new TADA_Advance_ApplyDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }
        
        [HttpPost]
        [Route("TadaAllwence")]
        public TADA_Advance_ApplyDTO TadaAllwence([FromBody]TADA_Advance_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TadaAllwence(data);
        }
        [Route("savedta")]
        public TADA_Advance_ApplyDTO savedta([FromBody]TADA_Advance_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(data);
        }
        [Route("edit")]

        public TADA_Advance_ApplyDTO edit([FromBody]TADA_Advance_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edit(data);
        }
        //deactive
        [Route("deactive")]

        public TADA_Advance_ApplyDTO deactive([FromBody]TADA_Advance_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
        //getstate
        [Route("getstate")]

        public TADA_Advance_ApplyDTO getstate([FromBody]TADA_Advance_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getstate(data);
        }
    }
}