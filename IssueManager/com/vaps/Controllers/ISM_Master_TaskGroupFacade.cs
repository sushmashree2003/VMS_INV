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
    public class ISM_Master_TaskGroupFacade : Controller
    {
        public ISM_Master_TaskGroupInterface inter;
        public ISM_Master_TaskGroupFacade(ISM_Master_TaskGroupInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public ISM_Master_TaskGroupDTO loaddata([FromBody] ISM_Master_TaskGroupDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public ISM_Master_TaskGroupDTO savedata([FromBody] ISM_Master_TaskGroupDTO data)
        {
            return inter.savedata(data);
        }
        [Route("EditData")]
        public ISM_Master_TaskGroupDTO Editdata([FromBody] ISM_Master_TaskGroupDTO data)
        {
            return inter.Editdata(data);
        }
        
        [Route("clientDecative")]
        public ISM_Master_TaskGroupDTO clientDecative([FromBody] ISM_Master_TaskGroupDTO data)
        {
            return inter.clientDecative(data);
        }
        [Route("DeptDeactive")]
        public ISM_Master_TaskGroupDTO DeptDeactive([FromBody] ISM_Master_TaskGroupDTO data)
        {
            return inter.DeptDeactive(data);
        }
        [Route("viewuploaddept")]
        public ISM_Master_TaskGroupDTO viewuploaddept([FromBody] ISM_Master_TaskGroupDTO data)
        {
            return inter.viewuploaddept(data);
        }

    }
}
