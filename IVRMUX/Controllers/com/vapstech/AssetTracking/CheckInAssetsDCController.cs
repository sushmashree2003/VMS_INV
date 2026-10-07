using IVRMUX.Delegates.com.vapstech.AssetTracking;
using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking
{
    [Route("api/[controller]")]
    public class CheckInAssetsDCController : Controller
    {
        CheckInAssetsDCDelegate _delegate = new CheckInAssetsDCDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public CheckInAssetsDCDTO getloaddata(int id)
        {
            CheckInAssetsDCDTO data = new CheckInAssetsDCDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getStore")]
        public CheckInAssetsDCDTO getStore([FromBody] CheckInAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getStore(data);
        }
        [Route("getitems")]
        public CheckInAssetsDCDTO getitems([FromBody] CheckInAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("getdetails")]
        public CheckInAssetsDCDTO getdetails([FromBody] CheckInAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getdetails(data);
        }
        

        [Route("savedetails")]
        public CheckInAssetsDCDTO savedetails([FromBody] CheckInAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public CheckInAssetsDCDTO deactive([FromBody] CheckInAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
  
       


    }
}
