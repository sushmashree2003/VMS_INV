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
    public class ViewOfDeploymentFacadeController : Controller
    {
        ViewOfDeploymentInterface _dep;
        public ViewOfDeploymentFacadeController(ViewOfDeploymentInterface dep)
        {
            _dep = dep;
        }
 

      
        [Route("getloaddata")]
        public ViewOfDeploymentDTO getloaddata([FromBody] ViewOfDeploymentDTO data)
        {
            return _dep.getloaddata(data);
        }
        [Route("gettaskslist")]
        public ViewOfDeploymentDTO gettaskslist([FromBody] ViewOfDeploymentDTO data)
        {
            return _dep.gettaskslist(data);
        }
        
        [Route("GetOnchangeData")]
        public ViewOfDeploymentDTO GetOnchangeData([FromBody]ViewOfDeploymentDTO data)
        {
            return _dep.GetOnchangeData(data);
        }
        [Route("GetOnchangeClient")]
        public ViewOfDeploymentDTO GetOnchangeClient([FromBody]ViewOfDeploymentDTO data)
        {
            return _dep.GetOnchangeClient(data);
        }
        [Route("savedata")]
        public ViewOfDeploymentDTO savedata([FromBody]ViewOfDeploymentDTO data)
        {
            return _dep.savedata(data);
        }
        //scope report
        [Route("getReport")]
        public ViewOfDeploymentDTO getReport([FromBody] ViewOfDeploymentDTO data)
        {
            return _dep.getReport(data);
        }
        [Route("approve_reject_scope")]
        public ViewOfDeploymentDTO approve_reject_scope([FromBody] ViewOfDeploymentDTO data)
        {
            return _dep.approve_reject_scope(data);
        }
        [Route("get_scope_tasklist_for_approval")]
        public ViewOfDeploymentDTO get_scope_tasklist_for_approval([FromBody] ViewOfDeploymentDTO data)
        {
            return _dep.get_scope_tasklist_for_approval(data);
        }
    }
}
