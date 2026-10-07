using AssetTrackingServiceHub.com.vaps.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class AT_System_Category_Checkpoint_MappingFacadeController : Controller
    {
        private readonly IAT_System_Category_Checkpoint_Mapping _interface;

        public AT_System_Category_Checkpoint_MappingFacadeController(IAT_System_Category_Checkpoint_Mapping inter)
        {
            _interface = inter;
        }

        [HttpPost]
        [Route("getdetails")]
        public async Task<AT_System_Category_Checkpoint_MappingDTO> GetDetails([FromBody] AT_System_Category_Checkpoint_MappingDTO request)
        {

            return await _interface.GetDetails(request);
        }

        [HttpPost]
        [Route("saverecord")]
        public async Task<AT_System_Category_Checkpoint_MappingDTO> SaveRecord([FromBody] AT_System_Category_Checkpoint_MappingDTO request)
        {
            return await _interface.SaveRecord(request);
        }


    }
}
