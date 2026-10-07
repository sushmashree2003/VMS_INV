using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;
using AssetTrackingServiceHub.com.vaps.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AssetTrackingServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class CheckInAssetsDCFacadeController : Controller
    {
        // GET: api/values
        CheckInAssetsDCInterface _AT;
        public CheckInAssetsDCFacadeController(CheckInAssetsDCInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public CheckInAssetsDCDTO getloaddata([FromBody] CheckInAssetsDCDTO data)
        {
            return _AT.getloaddata(data);
        }
        [Route("getStore")]
        public CheckInAssetsDCDTO getStore([FromBody] CheckInAssetsDCDTO data)
        {
            return _AT.getStore(data);
        }
        [Route("getitems")]
        public CheckInAssetsDCDTO getitems([FromBody] CheckInAssetsDCDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("getdetails")]
        public CheckInAssetsDCDTO getdetails([FromBody] CheckInAssetsDCDTO data)
        {
            return _AT.getdetails(data);
        }
        
        [Route("savedetails")]
        public CheckInAssetsDCDTO savedetails([FromBody] CheckInAssetsDCDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public CheckInAssetsDCDTO deactive([FromBody] CheckInAssetsDCDTO data)
        {
            return _AT.deactive(data);
        }
   
     




    }
}
