using PreadmissionDTOs.com.vaps.AssetTracking;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
    public interface IAT_System_Category_Checkpoint_Mapping
    {
        Task<AT_System_Category_Checkpoint_MappingDTO> GetDetails(AT_System_Category_Checkpoint_MappingDTO request);
        Task<AT_System_Category_Checkpoint_MappingDTO> SaveRecord(AT_System_Category_Checkpoint_MappingDTO request);
     
    }
}
