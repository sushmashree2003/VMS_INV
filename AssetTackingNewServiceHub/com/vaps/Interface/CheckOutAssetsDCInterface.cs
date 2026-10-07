using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Interface
{
  public interface CheckOutAssetsDCInterface
    {
        CheckOutAssetsDCDTO getloaddata(CheckOutAssetsDCDTO data);
        CheckOutAssetsDCDTO getitems(CheckOutAssetsDCDTO data);
        CheckOutAssetsDCDTO savedetails(CheckOutAssetsDCDTO data);
        CheckOutAssetsDCDTO deactive(CheckOutAssetsDCDTO data);
        CheckOutAssetsDCDTO getcontactperson(CheckOutAssetsDCDTO data);
       CheckOutAssetsDCDTO get_avaiablestock(CheckOutAssetsDCDTO data);//
       // CheckOutAssetsDCDTO editdcNew(CheckOutAssetsDCDTO data);
        //CheckOutAssetsDCDTO editdc(CheckOutAssetsDCDTO data);


    }
}
