using AssetTrackingServiceHub.com.vaps.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class HealthCheckupEntryFacadeController : Controller
    {
       private readonly IHealthCheckupEntry _Checkup_Entry;

        public HealthCheckupEntryFacadeController(IHealthCheckupEntry Checkup_Entry)
        {
            _Checkup_Entry= Checkup_Entry;
        }
        [HttpPost]
        [Route("getdetails")]
        public async Task<Health_Entry_CheckupDTO> GetDetails([FromBody] Health_Entry_CheckupDTO request)
        {
            return await _Checkup_Entry.GetDetails(request);
        }


        [HttpPost]
        [Route("savedata")]
        public async Task<Health_Entry_CheckupDTO> SaveData([FromBody] Health_Entry_CheckupDTO request)
        {
            return await _Checkup_Entry.SaveData(request);
        }


        [HttpPost]
        [Route("getcheckpoints")]
        public async Task<Health_Entry_CheckupDTO> GetCheckpoints([FromBody] Health_Entry_CheckupDTO request)
        {
            return await _Checkup_Entry.GetCheckpoints(request);
        }

        [HttpPost]
        [Route("firstLevelApproval")]
        public async Task<Health_Entry_CheckupDTO> FirstLevelApproval([FromBody] Health_Entry_CheckupDTO request)
        {
            return await _Checkup_Entry.GetApprovalDetails(request);
        }

        [HttpPost]
        [Route("saveApproval")]
        public async Task<Health_Entry_CheckupDTO> SaveApproval([FromBody] Health_Entry_CheckupDTO request)
        {
            return await _Checkup_Entry.SaveApproval(request);
        }

        [HttpPost]
        [Route("getProjectSummaryReport")]
        public async Task<Health_Entry_CheckupDTO> GetProjectSummaryReport([FromBody] Health_Entry_CheckupDTO request)
        {
            return await _Checkup_Entry.GetProjectSummaryReport(request);
        
        }

        [HttpPost]
        [Route("getSignOffReportDetails")]
        public async Task<Health_Entry_CheckupDTO> GetSignOffReportDetails([FromBody] Health_Entry_CheckupDTO request)
        {
            return await _Checkup_Entry.GetSignOffReportDetails(request);
        }

        [HttpPost]
        [Route("getDetailedCheckupReport")]
        public async Task<Health_Entry_CheckupDTO> GetDetailedCheckupReport([FromBody] Health_Entry_CheckupDTO request)
        {
            return await _Checkup_Entry.GetDetailedCheckupReport(request);
        }
    }
}
