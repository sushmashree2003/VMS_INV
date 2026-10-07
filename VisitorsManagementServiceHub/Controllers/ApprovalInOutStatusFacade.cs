using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using VisitorsManagementServiceHub.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VisitorsManagementServiceHub.Controllers
{
    [Route("api/[controller]")]
    public class ApprovalInOutStatusFacade : Controller
    {
        ApprovalInOutStatusInterface interobj;
        public ApprovalInOutStatusFacade(ApprovalInOutStatusInterface obj)
        {
            interobj = obj;
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

        // POST api/<controller>
        [HttpPost]
        [Route("getDetails")]
        public ApprovalInOutStatus_DTO getDetails([FromBody]ApprovalInOutStatus_DTO data)
        {
            return interobj.getDetails(data);
        }
        [Route("EditDetails")]
        public ApprovalInOutStatus_DTO EditDetails([FromBody] ApprovalInOutStatus_DTO id)
        {
            return interobj.EditDetails(id);
        }
        [Route("saveData")]
        public ApprovalInOutStatus_DTO saveData([FromBody]ApprovalInOutStatus_DTO data)
        {
            return interobj.saveData(data);
        }

        [Route("change_radiobutton")]
        public ApprovalInOutStatus_DTO change_radiobutton([FromBody]ApprovalInOutStatus_DTO data)
        {
            return interobj.change_radiobutton(data);
        }
    }
}
