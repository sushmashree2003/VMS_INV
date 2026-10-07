using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class EmailDepartmentMappingFacadeController : Controller
    {
        public EmailDepartmentMappingInterface _objinter;

        public EmailDepartmentMappingFacadeController(EmailDepartmentMappingInterface _obji)
        {
            _objinter = _obji;
        }

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

        [Route("getdetails")]
        public EmailDepartmentMappingDTO getdetails([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("onchangedept")]
        public EmailDepartmentMappingDTO onchangedept([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.onchangedept(data);
        }
        [Route("editdata")]
        public EmailDepartmentMappingDTO editdata([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.editdata(data);
        }
        [Route("savedata")]
        public EmailDepartmentMappingDTO savedata([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.savedata(data);
        }

        // Department Remarks Mapping 
        [Route("getloaddata")]
        public EmailDepartmentMappingDTO getloaddata([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.getloaddata(data);
        }
        [Route("onchangeinstitution")]
        public EmailDepartmentMappingDTO onchangeinstitution([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.onchangeinstitution(data);
        }
        [Route("onchangedepartment")]
        public EmailDepartmentMappingDTO onchangedepartment([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.onchangedepartment(data);
        }
        [Route("saveremarksdata")]
        public EmailDepartmentMappingDTO saveremarksdata([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.saveremarksdata(data);
        }
        [Route("Editremarksdata")]
        public EmailDepartmentMappingDTO Editremarksdata([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.Editremarksdata(data);
        }
        [Route("deactivate")]
        public EmailDepartmentMappingDTO deactivate([FromBody] EmailDepartmentMappingDTO data)
        {
            return _objinter.deactivate(data);
        }
    }
}
