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
    public class CheckOutAssetsDCFacadeController : Controller
    {
        // GET: api/values
        CheckOutAssetsDCInterface _AT;
        public CheckOutAssetsDCFacadeController(CheckOutAssetsDCInterface AT)
        {
            _AT = AT;
        }
        [Route("getloaddata")]
        public CheckOutAssetsDCDTO getloaddata([FromBody] CheckOutAssetsDCDTO data)
        {
            return _AT.getloaddata(data);
        }

        [Route("getitems")]
        public CheckOutAssetsDCDTO getitems([FromBody] CheckOutAssetsDCDTO data)
        {
            return _AT.getitems(data);
        }
        [Route("savedetails")]
        public CheckOutAssetsDCDTO savedetails([FromBody] CheckOutAssetsDCDTO data)
        {
            return _AT.savedetails(data);
        }
        [Route("deactive")]
        public CheckOutAssetsDCDTO deactive([FromBody] CheckOutAssetsDCDTO data)
        {
            return _AT.deactive(data);
        }
        [Route("getcontactperson")]
        public CheckOutAssetsDCDTO getcontactperson([FromBody] CheckOutAssetsDCDTO data)
        {
            return _AT.getcontactperson(data);
        }
        [Route("get_avaiablestock")]
        public CheckOutAssetsDCDTO get_avaiablestock([FromBody] CheckOutAssetsDCDTO data)
        {
            return _AT.get_avaiablestock(data);
        }
        //[Route("editdcNew")]
        //public CheckOutAssetsDCDTO editdcNew([FromBody] CheckOutAssetsDCDTO data)
        //{
        //    return _AT.editdcNew(data);
        //}
        //[Route("editdc")]
        //public CheckOutAssetsDCDTO editdc([FromBody] CheckOutAssetsDCDTO data)
        //{
        //    return _AT.editdc(data);
        //}



    }
}
 