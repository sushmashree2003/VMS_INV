using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IVRMUX.Controllers.com.vapstech.VMS.TADA
{
    [Produces("application/json")]
    [Route("api/TADA_CityMapping")]
    public class TADA_CityMappingController : Controller
    {

      public  TADA_CityMappinDelegate _delegate = new TADA_CityMappinDelegate();
      
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public TRDACityMappingDTO loaddata(int id)
        {
            TRDACityMappingDTO dto = new TRDACityMappingDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.loaddata(dto);
         


        }
        [HttpPost]
        [Route("saverecord")]
        public TRDACityMappingDTO saverecord([FromBody]TRDACityMappingDTO dto)
        {
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId= Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.saverecord(dto);

        }
        [Route("edit")]
        public TRDACityMappingDTO edit([FromBody]TRDACityMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));           
            return _delegate.edit(data);
        }
        [Route("deactive")]
        public TRDACityMappingDTO deactive([FromBody]TRDACityMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.deactive(data);
        }

        [Route("getcity")]
        public TRDACityMappingDTO getcity([FromBody]TRDACityMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delegate.getcity(data);
        }
        //
        [Route("getMasterReport")]
        public TRDACityMappingDTO getMasterReport([FromBody]TRDACityMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getMasterReport(data);
        }
    }
}