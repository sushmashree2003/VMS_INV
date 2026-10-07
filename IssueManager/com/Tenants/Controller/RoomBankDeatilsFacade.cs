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
    public class RoomBankDeatilsFacade : ControllerBase
    {
        public RoomBankDeatilsInterface _interface;

        public RoomBankDeatilsFacade(RoomBankDeatilsInterface _interf)
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
        public RoomBankDeatilsDTO getloaddata([FromBody] RoomBankDeatilsDTO data)
        {
            return _interface.getloaddata(data);
        }



        [Route("savedata")]
        public RoomBankDeatilsDTO savedetails([FromBody]RoomBankDeatilsDTO data)
        {
            return _interface.savedetails(data);
        }

        [Route("deactive")]        public RoomBankDeatilsDTO deactive([FromBody] RoomBankDeatilsDTO data)        {            return _interface.deactive(data);        }
    }
}

