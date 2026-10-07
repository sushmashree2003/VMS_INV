using PreadmissionDTOs.com.vaps.Inventory;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Master.Interface
{
    public interface IMasterSystem
    {
       Task<MasterSystemDTO> GetDetails(MasterSystemDTO request);
        Task<MasterSystemDTO> SaveOrUpdateRecord(MasterSystemDTO request);
        Task<MasterSystemDTO> DeleteRecord(MasterSystemDTO request);
    }
}
