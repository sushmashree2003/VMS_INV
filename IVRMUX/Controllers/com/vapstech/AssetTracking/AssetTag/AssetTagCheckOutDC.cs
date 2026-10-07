using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.AssetTracking.AssetTag
{
    [Route("api/[controller]")]
    public class AssetTagCheckOutDC : Controller
    {
        AssetTagCheckOutDCDelegate _delegate = new AssetTagCheckOutDCDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public AssetTagCheckOutDTO getloaddata(int id)
        {
            AssetTagCheckOutDTO data = new AssetTagCheckOutDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        [Route("getitems")]
        public AssetTagCheckOutDTO getitems([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("getitemtagdata")]
        public AssetTagCheckOutDTO getitemtagdata([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitemtagdata(data);
        }
        [Route("savedata")]
        public AssetTagCheckOutDTO savedata([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedata(data);
        }
        [Route("deactive")]
        public AssetTagCheckOutDTO deactive([FromBody] AssetTagCheckOutDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
    }
}
