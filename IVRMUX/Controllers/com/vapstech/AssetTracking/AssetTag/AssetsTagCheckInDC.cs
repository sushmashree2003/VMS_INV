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
    public class AssetsTagCheckInDC : Controller
    {
        AssetsTagCheckInDCDelegate _delegate = new AssetsTagCheckInDCDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public AssetsTagCheckInDTO getloaddata(int id)
        {
            AssetsTagCheckInDTO data = new AssetsTagCheckInDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getStore")]
        public AssetsTagCheckInDTO getStore([FromBody] AssetsTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getStore(data);
        }
        [Route("getitems")]
        public AssetsTagCheckInDTO getitems([FromBody] AssetsTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("getdetails")]
        public AssetsTagCheckInDTO getdetails([FromBody] AssetsTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getdetails(data);
        }


        [Route("savedetails")]
        public AssetsTagCheckInDTO savedetails([FromBody] AssetsTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public AssetsTagCheckInDTO deactive([FromBody] AssetsTagCheckInDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }

    }
}
