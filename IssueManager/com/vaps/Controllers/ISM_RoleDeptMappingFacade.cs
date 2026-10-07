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
    public class ISM_RoleDeptMappingFacade : Controller
    {
        public ISM_RoleDeptMappingInterface inter;
        public ISM_RoleDeptMappingFacade(ISM_RoleDeptMappingInterface s)
        {
            inter = s;
        }
        // GET: api/<controller>
        [Route("loaddata")]
        public ISM_RoleDeptMappingDTO loaddata([FromBody] ISM_RoleDeptMappingDTO data)
        {
            return inter.loaddata(data);
        }
        
        [Route("savedata")]
        public ISM_RoleDeptMappingDTO savedata([FromBody] ISM_RoleDeptMappingDTO data)
        {
            return inter.savedata(data);
        }
        [Route("deptDecative")]
        public ISM_RoleDeptMappingDTO deptDecative([FromBody] ISM_RoleDeptMappingDTO data)
        {
            return inter.deptDecative(data);
        }
    }
}
