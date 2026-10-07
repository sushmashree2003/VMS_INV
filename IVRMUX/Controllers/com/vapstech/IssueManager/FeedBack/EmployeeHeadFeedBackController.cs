 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.FeedBack;
using IVRMUX.Delegates.com.vapstech.IssueManager.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;
using PreadmissionDTOs.FeedBack;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.FeedBack
{
    [Route("api/[controller]")]
    public class EmployeeHeadFeedBackController : Controller
    {
        EmployeeHeadFeedBackDelegate _delg = new EmployeeHeadFeedBackDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }




        [Route("getdetails/{id:int}")]
     
        public EmployeeHeadFeedBackDTO getdetails(int id)
        {
            EmployeeHeadFeedBackDTO data = new EmployeeHeadFeedBackDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getdetails(data);
        }


        [Route("savedetails")]
        public EmployeeHeadFeedBackDTO savedetails([FromBody] EmployeeHeadFeedBackDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.savedetails(data);
        }




        [Route("delete_data/{id:int}")]
        public EmployeeHeadFeedBackDTO delete_data(int id)
        {
            return _delg.delete_data(id);
        }
    }
}