using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class CheckInAssetsDCDelegate
    {
        CommonDelegate<CheckInAssetsDCDTO, CheckInAssetsDCDTO> COMAT = new CommonDelegate<CheckInAssetsDCDTO, CheckInAssetsDCDTO>();
        public CheckInAssetsDCDTO getloaddata(CheckInAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsDCFacade/getloaddata/");
        }
        public CheckInAssetsDCDTO getStore(CheckInAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsDCFacade/getStore/");
        }
        public CheckInAssetsDCDTO getitems(CheckInAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsDCFacade/getitems/");
        }
        public CheckInAssetsDCDTO getdetails(CheckInAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsDCFacade/getdetails/");
        }
        public CheckInAssetsDCDTO savedetails(CheckInAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsDCFacade/savedetails/");
        }
        public CheckInAssetsDCDTO deactive(CheckInAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckInAssetsDCFacade/deactive/");
        }
      

        

    }
}
