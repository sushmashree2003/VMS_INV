using IVRMUX.Delegates.com.vapstech.AssetTracking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking
{
    [Route("api/[controller]")]
    public class AT_Master_CategoryController : Controller
    {
        AT_Master_CategoryDelegate _delegate = new AT_Master_CategoryDelegate();


        [Route("getdetails/{id:int}")]
        public AT_Master_CategoryDTO Getdetails(int id)
        {
            AT_Master_CategoryDTO dto = new AT_Master_CategoryDTO();
            dto.MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.GetDetails(dto);
        }

        [HttpPost]
        [Route("saverecord")]
        public AT_Master_CategoryDTO Saverecord([FromBody] AT_Master_CategoryDTO dto)
        {
            dto.MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.SaveDetail(dto);
        }

        [HttpPost]
        [Route("deactiveY")]
        public AT_Master_CategoryDTO DeactiveY([FromBody] AT_Master_CategoryDTO dto)
        {
            dto.MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.DeactiveY(dto);
        }
    }

   
    
}
