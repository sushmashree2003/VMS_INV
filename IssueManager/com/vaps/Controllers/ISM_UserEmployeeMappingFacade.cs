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
    public class ISM_UserEmployeeMappingFacade : Controller
    {
        // GET: api/<controller>

        public ISM_UserEmployeeMappingInterface _objinter;

        public ISM_UserEmployeeMappingFacade(ISM_UserEmployeeMappingInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<Clients_DTO> getdetails([FromBody] Clients_DTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getemployeelist")]
        public Clients_DTO getemployeelist([FromBody]Clients_DTO data)
        {
            return _objinter.getemployeelist(data);
        }


        [Route("savedata")]
        public Clients_DTO savedata([FromBody]Clients_DTO data)
        {
            return _objinter.savedata(data);
        }

        [Route("deactive")]
        public Clients_DTO deactive([FromBody]Clients_DTO data)
        {
            return _objinter.deactive(data);
        }
        [Route("updateorder")]
        public Clients_DTO updateorder([FromBody]Clients_DTO data)
        {
            return _objinter.updateorder(data);
        }
        [Route("getdetailsNew")]
        public Task<Clients_DTO> getdetailsNew([FromBody] Clients_DTO data)
        {
            return _objinter.getdetailsNew(data);
        }

        [Route("getUserEmpMappedReport")]
        public Task<Clients_DTO> getUserEmpMappedReport([FromBody] Clients_DTO data)
        {
            return _objinter.getUserEmpMappedReport(data);
        }
    }
}
