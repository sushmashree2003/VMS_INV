using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface CheckInAssetsDCInterface
    {
        CheckInAssetsDCDTO getloaddata(CheckInAssetsDCDTO data);
        CheckInAssetsDCDTO getStore(CheckInAssetsDCDTO data);
        CheckInAssetsDCDTO getitems(CheckInAssetsDCDTO data);
        CheckInAssetsDCDTO getdetails(CheckInAssetsDCDTO data);        
        CheckInAssetsDCDTO savedetails(CheckInAssetsDCDTO data);
        CheckInAssetsDCDTO deactive(CheckInAssetsDCDTO data);
    
        
    }
}
