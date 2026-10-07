using PreadmissionDTOs.com.vaps.AssetTracking;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface IAT_Master_CheckPoint
    {
        Task<AT_Master_CheckPointDTO> GetDetails(AT_Master_CheckPointDTO request);
        Task<AT_Master_CheckPointDTO> SaveOrUpdateRecord(AT_Master_CheckPointDTO request);
        Task<AT_Master_CheckPointDTO> DeleteRecord(AT_Master_CheckPointDTO request);
    }
}
