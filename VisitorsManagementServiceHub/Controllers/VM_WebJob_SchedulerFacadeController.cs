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
    public class VM_WebJob_SchedulerFacadeController : Controller
    {
        VM_WebJob_SchedulerInterface _interface;
        public VM_WebJob_SchedulerFacadeController(VM_WebJob_SchedulerInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("vmwebjobscheduler")] 
        public VM_webJob_SchedulerDTO vmwebjobscheduler([FromBody]VM_webJob_SchedulerDTO data)
        {
            return _interface.vmwebjobscheduler(data);
        }       

        [Route("vmhourwebjobscheduler")] 
        public VM_webJob_SchedulerDTO vmhourwebjobscheduler([FromBody]VM_webJob_SchedulerDTO data)
        {
            return _interface.vmhourwebjobscheduler(data);
        }

        [Route("vmminwebjobscheduler")] 
        public VM_webJob_SchedulerDTO vmminwebjobscheduler([FromBody]VM_webJob_SchedulerDTO data)
        {
            return _interface.vmminwebjobscheduler(data);
        }
    }
}
