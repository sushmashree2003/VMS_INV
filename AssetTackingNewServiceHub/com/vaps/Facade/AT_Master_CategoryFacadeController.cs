using AssetTrackingServiceHub.com.vaps.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Facade
{
    [Route("api/[Controller]")]
    public class AT_Master_CategoryFacadeController : Controller
    {
        private readonly IMasterCategory _master;

        public AT_Master_CategoryFacadeController(IMasterCategory master)
        {
            _master = master;
        }

        [HttpPost]
        [Route("getdetails")]
        public async Task<AT_Master_CategoryDTO> GetDetails([FromBody] AT_Master_CategoryDTO request)
        {
            return await _master.GetDetails(request);
        }

        [HttpPost]
        [Route("saverecord")]
        public async Task<AT_Master_CategoryDTO> SaveDetail([FromBody] AT_Master_CategoryDTO request)
        {
            return await _master.SaveOrUpdateRecord(request);
        }

        [HttpPost]
        [Route("deactiveY")]
        public async Task<AT_Master_CategoryDTO> DeactiveY([FromBody] AT_Master_CategoryDTO request)
        {
            return await _master.DeleteRecord(request);
        }
    }
}
    
    
