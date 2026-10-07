using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IssueManager.com.TADA.Controllers
{
    //[Produces("application/json")]
    //[Route("api/TADA_CityMappingFacadE")]
    [Route("api/[controller]")]
    public class TADA_CityMappingFacadEController : Controller
    {
        public TADACityMappingInterface _objinter;

        public TADA_CityMappingFacadEController(TADACityMappingInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("loaddata")]
        public TRDACityMappingDTO loaddata([FromBody]TRDACityMappingDTO data)
        {
            return _objinter.loaddata(data);
        }
        [Route("saverecord")]
        public TRDACityMappingDTO saverecord([FromBody]TRDACityMappingDTO value)
        {
            return _objinter.saverecord(value);
        }
        [Route("getcity")]
        public TRDACityMappingDTO getcity([FromBody]TRDACityMappingDTO value)
        {
            return _objinter.getcity(value);
        }
        [Route("edit")]
        public TRDACityMappingDTO edit([FromBody]TRDACityMappingDTO value)
        {
            return _objinter.edit(value);
        }
        //deactive
        [Route("deactive")]
        public TRDACityMappingDTO deactive([FromBody]TRDACityMappingDTO value)
        {
            return _objinter.deactive(value);
        }
        //getMasterReport
        [Route("getMasterReport")]
        public TRDACityMappingDTO getMasterReport([FromBody]TRDACityMappingDTO value)
        {
            return _objinter.getMasterReport(value);
        }
        //getcompany
        //[Route("getcompany")]
        //public TRDACityMappingDTO getcompany([FromBody]TRDACityMappingDTO value)
        //{
        //    return _objinter.getcompany(value);
        //}
    }
}