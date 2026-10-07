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
    public class UserMappingFacade : Controller
    {
        // GET: api/<controller>

        public UserMappingInterface _objinter;

        public UserMappingFacade(UserMappingInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Clients_DTO getdetails([FromBody] Clients_DTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("saveClientMappingdata")]
        public Clients_DTO saveClientMappingdata([FromBody]Clients_DTO data)
        {
            return _objinter.saveClientMappingdata(data);
        }

        [Route("deactiveClientMappingdata")]
        public Clients_DTO deactiveClientMappingdata([FromBody]Clients_DTO data)
        {
            return _objinter.deactiveClientMappingdata(data);
        }

        [Route("editClientMappingdata")]
        public Clients_DTO editClientMappingdata([FromBody]Clients_DTO data)
        {
            return _objinter.editClientMappingdata(data);
        }

        [Route("get_MappedUserlist")]
        public Clients_DTO get_MappedUserlist([FromBody]Clients_DTO data)
        {
            return _objinter.get_MappedUserlist(data);
        }
        
    }
}
