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
    [Route("api/TADA_Apply")]
    public class TADA_ApplyController : Controller
    {
        TADA_ApplyDelegate _delegate = new TADA_ApplyDelegate();


        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADA_ApplyDTO getalldata(int id)
        {
            TADA_ApplyDTO dto = new TADA_ApplyDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }
        [HttpPost]
        [Route("TadaAllwence")]
        public TADA_ApplyDTO TadaAllwence([FromBody]TADA_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TadaAllwence(data);
        }
        [HttpPost]
        [Route("Viewdetails")]
        public TADA_ApplyDTO Viewdetails([FromBody]TADA_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Viewdetails(data);
        }
        [HttpPost]
        [Route("savedta")]
        public TADA_ApplyDTO savedta([FromBody]TADA_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(data);
        }
        [HttpPost]
        [Route("edit")]
        public TADA_ApplyDTO edit([FromBody]TADA_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edit(data);
        }
        //edit
        [HttpPost]
        [Route("deactive")]

        public TADA_ApplyDTO deactive([FromBody]TADA_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
        //Getdata
        [HttpPost]
        [Route("Getdata")]
        public TADA_ApplyDTO Getdata([FromBody]TADA_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Getdata(data);
        }
        [Route("getstate")]
        public TADA_ApplyDTO getstate([FromBody]TADA_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getstate(data);
        }
        //Updatefile
        [Route("Updatefile")]
        public TADA_ApplyDTO Updatefile([FromBody]TADA_ApplyDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Updatefile(data);
        }
    }
}