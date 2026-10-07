using System;
using System.Collections.Generic;
using System.Linq;
using IssueManager.com.Tenants.Interface;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.Tenants.Facade
{
    [Route("api/[controller]")]
    public class RoomWiseAmenitiesMappingFacade : ControllerBase
    {
        public RoomWiseAmenitiesMappingInterface _interface;

        public RoomWiseAmenitiesMappingFacade(RoomWiseAmenitiesMappingInterface _interf)
        {
            _interface = _interf;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    
        [Route("getloaddata")]
        public RoomWiseAmenitiesMappingDTO getloaddata([FromBody] RoomWiseAmenitiesMappingDTO data)
        {
            return _interface.getloaddata(data);
        }



        [Route("savedata")]
        public RoomWiseAmenitiesMappingDTO savedetails([FromBody]RoomWiseAmenitiesMappingDTO data)
        {
            return _interface.savedetails(data);
        }

        [Route("deactive")]        public RoomWiseAmenitiesMappingDTO deactive([FromBody] RoomWiseAmenitiesMappingDTO data)        {            return _interface.deactive(data);        }
        [Route("Edit")]        public RoomWiseAmenitiesMappingDTO Edit([FromBody] RoomWiseAmenitiesMappingDTO data)        {            return _interface.Edit(data);        }
    }
}
