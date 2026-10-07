using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface AT_MasterSiteInterface
    {
        AT_MasterSiteDTO getloaddata(AT_MasterSiteDTO data);
        AT_MasterSiteDTO savedetails(AT_MasterSiteDTO data);
        AT_MasterSiteDTO deactive(AT_MasterSiteDTO data);
        Task<MasterSystemDTO> GetDetails(MasterSystemDTO request);
        Task<MasterSystemDTO> SaveOrUpdateRecord(MasterSystemDTO request);
        Task<MasterSystemDTO> DeleteRecord(MasterSystemDTO request);
    }
}
