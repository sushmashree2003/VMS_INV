using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class CheckOutAssetsDCDelegate
    {
        CommonDelegate<CheckOutAssetsDCDTO, CheckOutAssetsDCDTO> COMAT = new CommonDelegate<CheckOutAssetsDCDTO, CheckOutAssetsDCDTO>();
        public CheckOutAssetsDCDTO getloaddata(CheckOutAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsDCFacade/getloaddata/");
        }
        public CheckOutAssetsDCDTO getitems(CheckOutAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsDCFacade/getitems/");
        }
        
        public CheckOutAssetsDCDTO savedetails(CheckOutAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsDCFacade/savedetails/");
        }
        public CheckOutAssetsDCDTO deactive(CheckOutAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsDCFacade/deactive/");
        }
        public CheckOutAssetsDCDTO getcontactperson(CheckOutAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsDCFacade/getcontactperson/");
        }
        public CheckOutAssetsDCDTO get_avaiablestock(CheckOutAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsDCFacade/get_avaiablestock/");
        }
        public CheckOutAssetsDCDTO editdc(CheckOutAssetsDCDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "CheckOutAssetsDCFacade/editdc/");
        }

    }
}
