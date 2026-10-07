using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using AssetTrackingServiceHub.com.vaps.Interface;
using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Controllers
{
    [Route("api/[controller]")]
    public class AssetTagFacade : Controller
    {
        // GET: api/values
        AssetTagInterface _AT;
        public AssetTagFacade(AssetTagInterface AT)
        {
            _AT = AT;
        }


        #region QRCODE
        [Route("create_inv_qrcode")]
        public AssetTagDTO create_inv_qrcode([FromBody] AssetTagDTO data)
        {
            return _AT.create_inv_qrcode(data);
        }
        [Route("LoadSite")]
        public AssetTagDTO LoadSite([FromBody] AssetTagDTO data)
        {
            return _AT.LoadSite(data);
        }
        [Route("LoadQRdata")]
        public AssetTagDTO LoadQRdata([FromBody] AssetTagDTO data)
        {
            return _AT.LoadQRdata(data);
        }
        [Route("Save_INV_qrremarks")]
        public AssetTagDTO Save_INV_qrremarks([FromBody] AssetTagDTO data)
        {
            return _AT.Save_INV_qrremarks(data);
        }
        [Route("INV_QR_Report")]
        public AssetTagDTO INV_QR_Report([FromBody] AssetTagDTO data)
        {
            return _AT.INV_QR_Report(data);
        }
        [Route("INV_QR_Report_User")]
        public AssetTagDTO INV_QR_Report_User([FromBody] AssetTagDTO data)
        {
            return _AT.INV_QR_Report_User(data);
        }
        [Route("INV_QR_View_Item")]
        public AssetTagDTO INV_QR_View_Item([FromBody] AssetTagDTO data)
        {
            return _AT.INV_QR_View_Item(data);
        }
        #endregion
        [Route("getloaddata")]
        public AssetTagDTO getloaddata([FromBody] AssetTagDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("getdata")]
        public Task<AssetTagDTO> getdata([FromBody] AssetTagDTO data)
        {
            return _AT.getdata(data);
        }
        [Route("savedata")]
        public AssetTagDTO savedata([FromBody] AssetTagDTO data)
        {
            return _AT.savedata(data);
        }
        [Route("deactive")]
        public AssetTagDTO deactive([FromBody] AssetTagDTO data)
        {
            return _AT.deactive(data);
        }
        [Route("get_edit_data")]
        public AssetTagDTO get_edit_data([FromBody] AssetTagDTO data)
        {
            return _AT.get_edit_data(data);
        }

    }
}
