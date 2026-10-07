using System;
using IVRMUX.Delegates.com.vapstech.AssetTracking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking
{
    [Route("api/[controller]")]
    public class MasterSystemController : Controller
    {
        private readonly MasterSystemDelegate _masterDelegate = new MasterSystemDelegate();

        [Route("Getdetails/{id:int}")]
        public MasterSystemDTO GetDetails(int id)
        {
            MasterSystemDTO masterSystemDTO = new MasterSystemDTO();
            masterSystemDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _masterDelegate.GetDetails(masterSystemDTO);
        }

        [Route("saverecord")]
        public MasterSystemDTO saveDetail([FromBody] MasterSystemDTO masterSystemDTO)
        {
            masterSystemDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            masterSystemDTO.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _masterDelegate.SaveDetail(masterSystemDTO);
        }
        [Route("deactiveY")]
        public MasterSystemDTO DeleteRecord([FromBody] MasterSystemDTO masterSystemDTO)
        {
            masterSystemDTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            masterSystemDTO.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _masterDelegate.DeleteData(masterSystemDTO);
        }
    }
}
