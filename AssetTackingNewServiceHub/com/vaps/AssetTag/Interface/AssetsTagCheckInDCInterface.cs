using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
   public interface AssetsTagCheckInDCInterface
    {
        AssetsTagCheckInDTO getloaddata(AssetsTagCheckInDTO data);
        AssetsTagCheckInDTO getStore(AssetsTagCheckInDTO data);
        AssetsTagCheckInDTO getitems(AssetsTagCheckInDTO data);
        AssetsTagCheckInDTO getdetails(AssetsTagCheckInDTO data);
        AssetsTagCheckInDTO savedetails(AssetsTagCheckInDTO data);
        AssetsTagCheckInDTO deactive(AssetsTagCheckInDTO data);
    }
}
