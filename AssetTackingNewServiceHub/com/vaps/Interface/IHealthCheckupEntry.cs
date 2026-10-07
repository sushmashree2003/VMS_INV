using PreadmissionDTOs.com.vaps.AssetTracking;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface IHealthCheckupEntry
    {
        Task<Health_Entry_CheckupDTO> GetDetails(Health_Entry_CheckupDTO request);
         Task<Health_Entry_CheckupDTO> SaveData(Health_Entry_CheckupDTO request);
        Task<Health_Entry_CheckupDTO> GetCheckpoints(Health_Entry_CheckupDTO request);
        Task<Health_Entry_CheckupDTO> GetApprovalDetails(Health_Entry_CheckupDTO request);
        Task<Health_Entry_CheckupDTO> SaveApproval(Health_Entry_CheckupDTO request);
        Task<Health_Entry_CheckupDTO> GetProjectSummaryReport(Health_Entry_CheckupDTO request);
        Task<Health_Entry_CheckupDTO> GetSignOffReportDetails(Health_Entry_CheckupDTO request);
        Task<Health_Entry_CheckupDTO> GetDetailedCheckupReport(Health_Entry_CheckupDTO request);
    }
}
