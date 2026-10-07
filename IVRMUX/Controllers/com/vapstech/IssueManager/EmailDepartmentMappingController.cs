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
    public class EmailDepartmentMappingController : Controller
    {
        EmailDepartmentMappingDelegate _delg = new EmailDepartmentMappingDelegate();

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
        [Route("getdetails/{id:int}")]
        public EmailDepartmentMappingDTO getdetails(int id)
        {
            EmailDepartmentMappingDTO data = new EmailDepartmentMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getdetails(data);
        }

        [Route("onchangedept")]
        public EmailDepartmentMappingDTO onchangedept([FromBody] EmailDepartmentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangedept(data);
        }
        [Route("editdata")]
        public EmailDepartmentMappingDTO editdata([FromBody] EmailDepartmentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.editdata(data);
        }

        [Route("savedata")]
        public EmailDepartmentMappingDTO savedata([FromBody] EmailDepartmentMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.savedata(data);
        }
        // Department Remarks Mapping 

        [Route("getloaddata/{id:int}")]
        public EmailDepartmentMappingDTO getloaddata(int id)
        {
            EmailDepartmentMappingDTO data = new EmailDepartmentMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getloaddata(data);
        }
        [Route("onchangeinstitution")]
        public EmailDepartmentMappingDTO onchangeinstitution([FromBody]  EmailDepartmentMappingDTO data)
        { 
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeinstitution(data);
        }
        [Route("onchangedepartment")]
        public EmailDepartmentMappingDTO onchangedepartment([FromBody]  EmailDepartmentMappingDTO data)
        { 
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangedepartment(data);
        }

        [Route("saveremarksdata")]
        public EmailDepartmentMappingDTO saveremarksdata([FromBody]  EmailDepartmentMappingDTO data)
        { 
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.saveremarksdata(data);
        }

        [Route("Editremarksdata")]
        public EmailDepartmentMappingDTO Editremarksdata([FromBody]  EmailDepartmentMappingDTO data)
        { 
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.Editremarksdata(data);
        }
        [Route("deactivate")]
        public EmailDepartmentMappingDTO deactivate([FromBody]  EmailDepartmentMappingDTO data)
        { 
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.deactivate(data);
        }

    }
}
