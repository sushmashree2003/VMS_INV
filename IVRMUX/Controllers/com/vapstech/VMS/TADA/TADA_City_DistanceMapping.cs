using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.TADA
{
    [Route("api/[controller]")]
    public class TADA_City_DistanceMapping : Controller
    {
        TADA_City_DistanceMappingDelegate _delegate = new TADA_City_DistanceMappingDelegate();


        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADACityDistanceMappingDTO getalldata(int id)
        {
            TADACityDistanceMappingDTO dto = new TADACityDistanceMappingDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);
        }
        [Route("OnchangeCity")]
        public TADACityDistanceMappingDTO OnchangeCity([FromBody] TADACityDistanceMappingDTO data)
        {
       
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
       
            return _delegate.OnchangeCity(data);
        }
        [Route("savedata")]
        public TADACityDistanceMappingDTO savedata([FromBody] TADACityDistanceMappingDTO data)
        {
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedata(data);
        }
        [Route("EditData")]
        public TADACityDistanceMappingDTO EditData([FromBody] TADACityDistanceMappingDTO data)
        {
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.EditData(data);
        }
        [Route("TravelDetails")]
        public TADACityDistanceMappingDTO TravelDetails([FromBody] TADACityDistanceMappingDTO data)
        {
            //TADACityDistanceMappingDTO dto = new TADACityDistanceMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TravelDetails(data);
        }
        [Route("GetDesignation")]
        public TADACityDistanceMappingDTO GetDesignation([FromBody] TADACityDistanceMappingDTO data)
        {
            //TADACityDistanceMappingDTO dto = new TADACityDistanceMappingDTO();
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.GetDesignation(data);
        }
        [Route("Savedetails")]
        public TADACityDistanceMappingDTO Savedetails([FromBody] TADACityDistanceMappingDTO data)
        {
            //TADACityDistanceMappingDTO dto = new TADACityDistanceMappingDTO();
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Savedetails(data);
        }
        [Route("EditDetails")]
        public TADACityDistanceMappingDTO EditDetails([FromBody] TADACityDistanceMappingDTO data)
        {
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.EditDetails(data);
        }
        [Route("Getcities")]
        public TADACityDistanceMappingDTO Getcities([FromBody] TADACityDistanceMappingDTO data)
        {
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Getcities(data);
        }
        [Route("SaveTierdata")]
        public TADACityDistanceMappingDTO SaveTierdata([FromBody] TADACityDistanceMappingDTO data)
        {
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.SaveTierdata(data);
        }

    }
}
