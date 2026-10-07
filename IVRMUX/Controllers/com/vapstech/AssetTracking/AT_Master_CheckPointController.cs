using IVRMUX.Delegates.com.vapstech.AssetTracking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking
{
    [Route("api/[controller]")]
    public class AT_Master_CheckPointController : Controller
    {
        AT_Master_CheckPointDelegate _delegate = new AT_Master_CheckPointDelegate();

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public AT_Master_CheckPointDTO Getdetails(int id)
        {
            AT_Master_CheckPointDTO dto = new AT_Master_CheckPointDTO();
            dto.MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.GetDetails(dto);
        }

        [HttpPost]
        [Route("saverecord")]
        public AT_Master_CheckPointDTO Saverecord([FromBody] AT_Master_CheckPointDTO dto)
        {
            dto.MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.SaveDetail(dto);
        }

        [HttpPost]
        [Route("deactiveY")]
        public AT_Master_CheckPointDTO DeactiveY([FromBody] AT_Master_CheckPointDTO dto)
        {
            dto.MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.DeactiveY(dto);
        }
    }
}
