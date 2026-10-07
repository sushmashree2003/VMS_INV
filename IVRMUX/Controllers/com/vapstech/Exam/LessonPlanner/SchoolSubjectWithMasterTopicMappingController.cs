using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Exam.LessonPlanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Exam.LessonPlanner
{
    [Route("api/[controller]")]
    public class SchoolSubjectWithMasterTopicMappingController : Controller
    {
        public SchoolSubjectWithMasterTopicMappingDelegate _delg = new SchoolSubjectWithMasterTopicMappingDelegate();
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
        public SchoolSubjectWithMasterTopicMappingDTO Getdetails(int id)
        {
            SchoolSubjectWithMasterTopicMappingDTO data = new SchoolSubjectWithMasterTopicMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.Getdetails(data);
        }
        [Route("savedetails")]
        public SchoolSubjectWithMasterTopicMappingDTO savedetails([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedetails(data);
        }      

        [Route("onchangesubject")]
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubject([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesubject(data);
        }  

        [Route("deactivate")]
        public SchoolSubjectWithMasterTopicMappingDTO deactivate([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }

        [Route("editdeatils")]
        public SchoolSubjectWithMasterTopicMappingDTO editdeatils([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.editdeatils(data);
        }

        [Route("validateordernumber")]
        public SchoolSubjectWithMasterTopicMappingDTO validateordernumber([FromBody]   SchoolSubjectWithMasterTopicMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.validateordernumber(data);
        }
    }
}
