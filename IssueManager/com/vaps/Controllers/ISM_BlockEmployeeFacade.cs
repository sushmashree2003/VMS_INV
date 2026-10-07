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
    public class ISM_BlockEmployeeFacadeController : Controller
    {
        public ISM_BlockEmployeeInterface _objinter;

        public ISM_BlockEmployeeFacadeController(ISM_BlockEmployeeInterface _obji)
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

        [Route("getloaddata")]
        public Task<ISM_BlockEmployeeDTO> getloaddata([FromBody] ISM_BlockEmployeeDTO data)
        {
            return _objinter.getloaddata(data);
        }


        [Route("savedata")]
        public ISM_BlockEmployeeDTO savedata([FromBody] ISM_BlockEmployeeDTO data)
        {
            return _objinter.savedata(data);
        }
        [Route("getblockedEmpDeatils")]
        public ISM_BlockEmployeeDTO getblockedEmpDeatils([FromBody] ISM_BlockEmployeeDTO data)
        {
            return _objinter.getblockedEmpDeatils(data);
        }

        [Route("deactivate")]
        public ISM_BlockEmployeeDTO deactivate([FromBody] ISM_BlockEmployeeDTO data)
        {
            return _objinter.deactivate(data);
        }
    }
}
