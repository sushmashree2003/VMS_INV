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
    public class CheckOutAssetsDCController : Controller
    {
        CheckOutAssetsDCDelegate _delegate = new CheckOutAssetsDCDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public CheckOutAssetsDCDTO getloaddata(int id)
        {
            CheckOutAssetsDCDTO data = new CheckOutAssetsDCDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("getitems")]
        public CheckOutAssetsDCDTO getitems([FromBody] CheckOutAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getitems(data);
        }
        [Route("savedetails")]
        public CheckOutAssetsDCDTO savedetails([FromBody] CheckOutAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delegate.savedetails(data);
        }

        [Route("deactive")]
        public CheckOutAssetsDCDTO deactive([FromBody] CheckOutAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.deactive(data);
        }
        [Route("getcontactperson")]
        public CheckOutAssetsDCDTO getcontactperson([FromBody] CheckOutAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getcontactperson(data);
        }
        [Route("get_avaiablestock")]
        public CheckOutAssetsDCDTO get_avaiablestock([FromBody] CheckOutAssetsDCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.get_avaiablestock(data);
        }



    }
}
