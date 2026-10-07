using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag
{
    public class AssetsTagCheckInDCDelegate
    {
        CommonDelegate<AssetsTagCheckInDTO, AssetsTagCheckInDTO> COMAT = new CommonDelegate<AssetsTagCheckInDTO, AssetsTagCheckInDTO>();
        public AssetsTagCheckInDTO getloaddata(AssetsTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetsTagCheckInDCFacade/getloaddata/");
        }
        public AssetsTagCheckInDTO getStore(AssetsTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetsTagCheckInDCFacade/getStore/");
        }
        public AssetsTagCheckInDTO getitems(AssetsTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetsTagCheckInDCFacade/getitems/");
        }
        public AssetsTagCheckInDTO getdetails(AssetsTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetsTagCheckInDCFacade/getdetails/");
        }
        public AssetsTagCheckInDTO savedetails(AssetsTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetsTagCheckInDCFacade/savedetails/");
        }
        public AssetsTagCheckInDTO deactive(AssetsTagCheckInDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetsTagCheckInDCFacade/deactive/");
        }

    }
}
