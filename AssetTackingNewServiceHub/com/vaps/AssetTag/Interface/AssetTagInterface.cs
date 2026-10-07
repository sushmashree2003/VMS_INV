using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Interface
{
    public interface AssetTagInterface
    {
        AssetTagDTO getloaddata(AssetTagDTO data);
        Task<AssetTagDTO> getdata(AssetTagDTO data);
        AssetTagDTO savedata(AssetTagDTO data);
        AssetTagDTO deactive(AssetTagDTO data);
        AssetTagDTO get_edit_data(AssetTagDTO data);

        AssetTagDTO create_inv_qrcode(AssetTagDTO data);
        AssetTagDTO LoadSite(AssetTagDTO data);
        AssetTagDTO LoadQRdata(AssetTagDTO data);
        AssetTagDTO Save_INV_qrremarks(AssetTagDTO data);
        AssetTagDTO INV_QR_Report(AssetTagDTO data);
        AssetTagDTO INV_QR_Report_User(AssetTagDTO data);
        AssetTagDTO INV_QR_View_Item(AssetTagDTO data);
    }
}
