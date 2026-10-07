
using AssetTrackingServiceHub.com.vaps.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Master.Facade
{
    [Route("api/[controller]")]
    public class MasterSystemFacadeController : Controller
    {
        private readonly IMasterSystem _master;

        public MasterSystemFacadeController(IMasterSystem master)
        {
            _master = master;
        }

        [HttpPost]
        [Route("Getdetails")]
        public async Task<MasterSystemDTO> GetDetails([FromBody] MasterSystemDTO masterSystemDTO)
        {
            return await _master.GetDetails(masterSystemDTO);
        }
        [HttpPost]
        [Route("saverecord")]
        public async Task<MasterSystemDTO> SaveDetail([FromBody] MasterSystemDTO masterSystemDTO)
        {
            return await _master.SaveOrUpdateRecord(masterSystemDTO);
        }

        [HttpPost]
        [Route("deactiveY")]
        public async Task<MasterSystemDTO> DeleteRecord([FromBody] MasterSystemDTO masterSystemDTO)
        {
            return await _master.DeleteRecord(masterSystemDTO);
        }


    }
}
