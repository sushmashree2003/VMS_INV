using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class RemaindersDetailsController : Controller
    {
        RemaindersDetailsDelegate _delg = new RemaindersDetailsDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        } 

        [Route("GetDetails/{id:int}")]
        public RemaindersDetailsDTO GetDetails(int id)
        {
            RemaindersDetailsDTO data = new RemaindersDetailsDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.GetDetails(data);
        }

        [Route("OnChangeClient")]
        public RemaindersDetailsDTO OnChangeClient([FromBody]  RemaindersDetailsDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.OnChangeClient(data);
        }

        [Route("SaveData")]
        public RemaindersDetailsDTO SaveData([FromBody]  RemaindersDetailsDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.SaveData(data);
        }
    }
}
