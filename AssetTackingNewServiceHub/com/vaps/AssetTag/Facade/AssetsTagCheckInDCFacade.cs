using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetTrackingServiceHub.com.vaps.AssetTag.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Facade
{
    [Route("api/[controller]")]
    public class AssetsTagCheckInDCFacade : Controller
    {
        AssetsTagCheckInDCInterface _AT;
        public AssetsTagCheckInDCFacade(AssetsTagCheckInDCInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public AssetsTagCheckInDTO getloaddata([FromBody] AssetsTagCheckInDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("getStore")]
        public AssetsTagCheckInDTO getStore([FromBody] AssetsTagCheckInDTO data)
        {
            return _AT.getStore(data);
        }
        [Route("getitems")]
        public AssetsTagCheckInDTO getitems([FromBody] AssetsTagCheckInDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("getdetails")]
        public AssetsTagCheckInDTO getdetails([FromBody] AssetsTagCheckInDTO data)
        {
            return _AT.getdetails(data);
        }

        [Route("savedetails")]
        public AssetsTagCheckInDTO savedetails([FromBody] AssetsTagCheckInDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public AssetsTagCheckInDTO deactive([FromBody] AssetsTagCheckInDTO data)
        {
            return _AT.deactive(data);
        }

    }
}
