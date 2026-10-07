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
    public class ISM_CommonTaskController : Controller
    {

        public ISM_CommonTaskDelegate _delObj = new ISM_CommonTaskDelegate();

        [Route("Loaddata/{id:int}")]
        public ISM_CommonTask_DTO Loaddata(int id)
        {
            ISM_CommonTask_DTO data = new ISM_CommonTask_DTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _delObj.Loaddata(data);
        }
       
        [Route("get_Designation")]
        public ISM_CommonTask_DTO get_Designation([FromBody]ISM_CommonTask_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delObj.get_Designation(data);
        }

        [Route("get_Employee")]
        public ISM_CommonTask_DTO get_Employee([FromBody]ISM_CommonTask_DTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delObj.get_Employee(data);
        }
    }
}
