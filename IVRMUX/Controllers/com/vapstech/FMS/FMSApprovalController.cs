using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FMS
{
    [Route("api/[controller]")]
    public class FMSApprovalController : Controller
    {
        FMSApprovalDelegate _delg = new FMSApprovalDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [Route("loaddata/{id:int}")]
        public FMSApprovalDTO loaddata(int id)
        {
            FMSApprovalDTO data = new FMSApprovalDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.loaddata(data);
        }

        [Route("getdatas")]
        public FMSApprovalDTO getdatas([FromBody]FMSApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getdatas(data);
        }
        
        [Route("savedetails")]
        public FMSApprovalDTO savedetails([FromBody] FMSApprovalDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.savedetails(data);
        }
    }
}
