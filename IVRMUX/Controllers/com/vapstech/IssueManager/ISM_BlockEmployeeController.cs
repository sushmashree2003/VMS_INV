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
    public class ISM_BlockEmployeeController : Controller
    {
        ISM_BlockEmployeeDelegate _delg = new ISM_BlockEmployeeDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

   
        [Route("getloaddata")]
        public ISM_BlockEmployeeDTO getloaddata(ISM_BlockEmployeeDTO data)
        {           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getloaddata(data);
        }
          
        [Route("savedata")]
        public ISM_BlockEmployeeDTO savedata([FromBody]  ISM_BlockEmployeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedata(data);
        }
     [Route("getblockedEmpDeatils")]
        public ISM_BlockEmployeeDTO getblockedEmpDeatils([FromBody]  ISM_BlockEmployeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getblockedEmpDeatils(data);
        }
     
        [Route("deactivate")]
        public ISM_BlockEmployeeDTO deactivate([FromBody]  ISM_BlockEmployeeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }

    }
}
