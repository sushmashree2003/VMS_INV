using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class RemaindersDetailsFacadeController :Controller
    {
        public RemaindersDetailsInterface _interface; 

        public RemaindersDetailsFacadeController(RemaindersDetailsInterface _inter)
        {
            _interface = _inter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetDetails")]
        public RemaindersDetailsDTO GetDetails([FromBody]  RemaindersDetailsDTO data)
        {            
            return _interface.GetDetails(data);
        }

        [Route("OnChangeClient")]
        public RemaindersDetailsDTO OnChangeClient([FromBody]  RemaindersDetailsDTO data)
        {           
            return _interface.OnChangeClient(data);
        }

        [Route("SaveData")]
        public RemaindersDetailsDTO SaveData([FromBody]  RemaindersDetailsDTO data)
        { 
            return _interface.SaveData(data);
        }

        [Route("ClientSubscriptionMail")]
        public RemaindersDetailsDTO ClientSubscriptionMail([FromBody]  RemaindersDetailsDTO data)
        {
            return _interface.ClientSubscriptionMail(data);
        }
    }
}
