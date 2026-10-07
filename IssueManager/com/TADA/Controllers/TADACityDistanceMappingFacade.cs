using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.TADA.Controllers
{
    [Route("api/[controller]")]
    public class TADACityDistanceMappingFacade : Controller
    {
        public TADACityDistanceMappingInterface _objinter;

        public TADACityDistanceMappingFacade(TADACityDistanceMappingInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADACityDistanceMappingDTO getalldata([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("OnchangeCity")]
        public TADACityDistanceMappingDTO OnchangeCity([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("savedata")]
        public TADACityDistanceMappingDTO savedata([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.savedata(data);
        }
        [Route("EditData")]
        public TADACityDistanceMappingDTO EditData([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.EditData(data);
        }
        [Route("TravelDetails")]
        public TADACityDistanceMappingDTO TravelDetails([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.TravelDetails(data);
        }
        [Route("GetDesignation")]
        public TADACityDistanceMappingDTO GetDesignation([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.GetDesignation(data);
        }
        [Route("Savedetails")]
        public TADACityDistanceMappingDTO Savedetails([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.Savedetails(data);
        }
        [Route("EditDetails")]
        public TADACityDistanceMappingDTO EditDetails([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.EditDetails(data);
        }
        [Route("Getcities")]
        public TADACityDistanceMappingDTO Tocities([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.Getcities(data);
        }
        [Route("SaveTierdata")]
        public TADACityDistanceMappingDTO SaveTierdata([FromBody]TADACityDistanceMappingDTO data)
        {
            return _objinter.SaveTierdata(data);
        }
    }
}
