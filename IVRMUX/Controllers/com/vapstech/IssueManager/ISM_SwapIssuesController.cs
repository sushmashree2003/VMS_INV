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
    public class ISM_SwapIssuesController : Controller
    {
        ISM_SwapIssuesDelegate _delg = new ISM_SwapIssuesDelegate();

  
        

        [Route("loaddata/{id:int}")]
        public ISM_SwapIssuesDTO loaddata(int id)
        {
            ISM_SwapIssuesDTO data = new ISM_SwapIssuesDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.DR_Notification = Convert.ToInt64(HttpContext.Session.GetInt32("DR_Notification"));
            return _delg.loaddata(data);
        }


        [Route("Report")]
        public ISM_SwapIssuesDTO Report([FromBody]ISM_SwapIssuesDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.Report(data);
        }
        
        [Route("Swaplist")]
        public ISM_SwapIssuesDTO Swaplist([FromBody]ISM_SwapIssuesDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.Swaplist(data);
        }

         [Route("Swaptask")]
        public ISM_SwapIssuesDTO Swaptask([FromBody]ISM_SwapIssuesDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.Swaptask(data);
        }



    }
}
