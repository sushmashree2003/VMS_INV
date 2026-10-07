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
    public class HW_HeathCheckUpFacadeController : Controller
    {
        public HW_HeathCheckUpInterface _interface;

        public HW_HeathCheckUpFacadeController(HW_HeathCheckUpInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [Route("OnLoadHWHealthCheckUp")]
        public HW_HeathCheckUpDTO OnLoadHWHealthCheckUp([FromBody] HW_HeathCheckUpDTO data)
        { 
            return _interface.OnLoadHWHealthCheckUp(data);
        }

        [Route("SaveHWHealthChekUp")]
        public HW_HeathCheckUpDTO SaveHWHealthChekUp([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.SaveHWHealthChekUp(data);
        }


        [Route("welclosesavedata")]
        public HW_HeathCheckUpDTO welclosesavedata([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.welclosesavedata(data);
        }


        //added by praveenkumar Proposal Signoff

        [Route("ProposalSignOff")]
        public HW_HeathCheckUpDTO ProposalSignOff([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.ProposalSignOff(data);
        }

        [Route("EditHealthCheckUp")]
        public HW_HeathCheckUpDTO EditHealthCheckUp([FromBody] HW_HeathCheckUpDTO data)
        { 
            return _interface.EditHealthCheckUp(data);
        }

        [Route("ViewHealthCheckUpFiles")]
        public HW_HeathCheckUpDTO ViewHealthCheckUpFiles([FromBody] HW_HeathCheckUpDTO data)
        { 
            return _interface.ViewHealthCheckUpFiles(data);
        }

        [Route("ActiveDeactiveHealthCheckUp")]
        public HW_HeathCheckUpDTO ActiveDeactiveHealthCheckUp([FromBody] HW_HeathCheckUpDTO data)
        { 
            return _interface.ActiveDeactiveHealthCheckUp(data);
        }

        [Route("DeleteHWHealthCheckUpFiles")]
        public HW_HeathCheckUpDTO DeleteHWHealthCheckUpFiles([FromBody] HW_HeathCheckUpDTO data)
        { 
            return _interface.DeleteHWHealthCheckUpFiles(data);
        }

        //Approval Process
        [Route("LoadHWHealthCheckUpdataApproval")]
        public HW_HeathCheckUpDTO LoadHWHealthCheckUpdataApproval([FromBody] HW_HeathCheckUpDTO data)
        { 
            return _interface.LoadHWHealthCheckUpdataApproval(data);
        }
        [Route("SaveHWHealthChekUpApproval")]
        public HW_HeathCheckUpDTO SaveHWHealthChekUpApproval([FromBody] HW_HeathCheckUpDTO data)
        { 
            return _interface.SaveHWHealthChekUpApproval(data);
        }

        [Route("ViewHealthCheckUpComments")]
        public HW_HeathCheckUpDTO ViewHealthCheckUpComments([FromBody] HW_HeathCheckUpDTO data)
        { 
            return _interface.ViewHealthCheckUpComments(data);
        }


        //healthcheckup report
        [Route("HealthCheckUpInstitution")]
        public HW_HeathCheckUpDTO HealthCheckUpInstitution([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.HealthCheckUpInstitution(data);
        }

        [Route("getdetails")]
        public Task<HW_HeathCheckUpDTO> getdetails([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.getdetails(data);
        }

        [Route("esculationmail")]
        public HW_HeathCheckUpDTO esculationmail([FromBody] HW_HeathCheckUpDTO data)
        {
            return _interface.esculationmail(data);
        }

    }
}
