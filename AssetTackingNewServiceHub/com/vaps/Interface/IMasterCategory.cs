
using PreadmissionDTOs.com.vaps.AssetTracking;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface IMasterCategory
    {
        Task<AT_Master_CategoryDTO> GetDetails(AT_Master_CategoryDTO request);
        Task<AT_Master_CategoryDTO> SaveOrUpdateRecord(AT_Master_CategoryDTO request);
        Task<AT_Master_CategoryDTO> DeleteRecord(AT_Master_CategoryDTO request);
    }
}
