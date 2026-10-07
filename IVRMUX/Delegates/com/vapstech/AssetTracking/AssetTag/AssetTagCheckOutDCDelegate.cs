using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking.AssetTag
{
    public class AssetTagCheckOutDCDelegate
    {
        CommonDelegate<AssetTagCheckOutDTO, AssetTagCheckOutDTO> COMAT = new CommonDelegate<AssetTagCheckOutDTO, AssetTagCheckOutDTO>();
        public AssetTagCheckOutDTO getloaddata(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutDCFacade/getloaddata/");
        }

        public AssetTagCheckOutDTO getitems(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutDCFacade/getitems/");
        }
        public AssetTagCheckOutDTO getitemtagdata(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutDCFacade/getitemtagdata/");
        }
        public AssetTagCheckOutDTO savedata(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutDCFacade/savedata/");
        }
        public AssetTagCheckOutDTO deactive(AssetTagCheckOutDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "AssetTagCheckOutDCFacade/deactive/");
        }
    }
}
