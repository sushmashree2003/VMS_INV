using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.College.Exam.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.College.Exam.LessonPlanner
{
    [Route("api/[controller]")]
    public class CollegeSubjetMasterTopicMappingController : Controller
    {
        public CollegeSubjetMasterTopicMappingDelegate _delg = new CollegeSubjetMasterTopicMappingDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("Getdetails/{id:int}")]
        public CollegeSubjetMasterTopicMappingDTO Getdetails(int id)
        {
            CollegeSubjetMasterTopicMappingDTO data = new CollegeSubjetMasterTopicMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }
        [Route("savedetails")]
        public CollegeSubjetMasterTopicMappingDTO savedetails([FromBody]   CollegeSubjetMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails(data);
        }

        [Route("onchangesubject")]
        public CollegeSubjetMasterTopicMappingDTO onchangesubject([FromBody]   CollegeSubjetMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesubject(data);
        }

        [Route("deactivate")]
        public CollegeSubjetMasterTopicMappingDTO deactivate([FromBody]   CollegeSubjetMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }

        [Route("editdeatils")]
        public CollegeSubjetMasterTopicMappingDTO editdeatils([FromBody]   CollegeSubjetMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.editdeatils(data);
        }

        [Route("validateordernumber")]
        public CollegeSubjetMasterTopicMappingDTO validateordernumber([FromBody]   CollegeSubjetMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.validateordernumber(data);
        }
    }
}
