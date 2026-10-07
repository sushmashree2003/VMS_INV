using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
    public interface AssetTagSMS_NotificationInterface
    {
       // void getdata(int id);
        AssetTagDTO getdata(AssetTagDTO dto);
        AssetTagDTO mailDemo(AssetTagDTO dto);
       
    }
}
