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
    public class ISM_Client_ComplaintsTaskListFacade : Controller
    {

        public ISM_Client_ComplaintsTaskListInterface _objinter;

        public ISM_Client_ComplaintsTaskListFacade(ISM_Client_ComplaintsTaskListInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_ComplaintsTaskListDTO> getdetails([FromBody] ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.getdetails(data);
        }
        [Route("gettaskdetails")]
        public Task<ISM_ComplaintsTaskListDTO> gettaskdetails([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.gettaskdetails(data);
        }
        [Route("getsearchtask")]
        public Task<ISM_ComplaintsTaskListDTO> getsearchtask([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.getsearchtask(data);
        }

    }
}
