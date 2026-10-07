using AssetTrackingServiceHub.com.vaps.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class AT_Master_CheckPointFacadeController : Controller
    {
        private readonly IAT_Master_CheckPoint _master;

        public AT_Master_CheckPointFacadeController(IAT_Master_CheckPoint master)
        {
            _master = master;
        }

        [HttpPost]
        [Route("Getdetails")]
        public async Task<AT_Master_CheckPointDTO> GetDetails([FromBody] AT_Master_CheckPointDTO request)
        {
            return await _master.GetDetails(request);
        }

        [HttpPost]
        [Route("saverecord")]
        public async Task<AT_Master_CheckPointDTO> SaveDetail([FromBody] AT_Master_CheckPointDTO request)
        {
            return await _master.SaveOrUpdateRecord(request);
        }

        [HttpPost]
        [Route("deactiveY")]
        public async Task<AT_Master_CheckPointDTO> DeactiveY([FromBody] AT_Master_CheckPointDTO request)
        {
            return await _master.DeleteRecord(request);
        }
    }
}
