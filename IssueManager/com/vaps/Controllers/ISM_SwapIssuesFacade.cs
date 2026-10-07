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
    public class ISM_SwapIssuesFacade : Controller
    {

        public ISM_SwapIssuesInterface _interface;

        public ISM_SwapIssuesFacade(ISM_SwapIssuesInterface _inter)
        {
            _interface = _inter;
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

        [Route("loaddata")]
        public ISM_SwapIssuesDTO loaddata([FromBody] ISM_SwapIssuesDTO data)
        {
            return _interface.loaddata(data);
        }

        [Route("Report")]
        public ISM_SwapIssuesDTO Report([FromBody] ISM_SwapIssuesDTO data)
        {
            return _interface.Report(data);
        }
        [Route("Swaplist")]
        public ISM_SwapIssuesDTO Swaplist([FromBody] ISM_SwapIssuesDTO data)
        {
            return _interface.Swaplist(data);
        }
        [Route("Swaptask")]
        public ISM_SwapIssuesDTO Swaptask([FromBody] ISM_SwapIssuesDTO data)
        {
            return _interface.Swaptask(data);
        }

    }
}
