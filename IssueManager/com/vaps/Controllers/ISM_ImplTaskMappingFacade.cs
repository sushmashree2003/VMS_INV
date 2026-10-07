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
    public class ISM_ImplTaskMappingFacade : Controller
    {

        public ISM_ImplTaskMappingInterface _objinter;

        public ISM_ImplTaskMappingFacade(ISM_ImplTaskMappingInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_ImplTaskMappingDTO> getdetails([FromBody] ISM_ImplTaskMappingDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("deptfilter")]
        public Task<ISM_ImplTaskMappingDTO> deptfilter([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.deptfilter(data);
        }

        [Route("gettaskdetails")]
        public Task<ISM_ImplTaskMappingDTO> gettaskdetails([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.gettaskdetails(data);
        }
        [Route("savePlanner")]
        public ISM_ImplTaskMappingDTO savePlanner([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.savePlanner(data);
        }
         [Route("saveTransfer")]
        public ISM_ImplTaskMappingDTO saveTransfer([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.saveTransfer(data);
        }
          [Route("saveResponse")]
        public ISM_ImplTaskMappingDTO saveResponse([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.saveResponse(data);
        }

        [Route("getsearchtask")]
        public Task<ISM_ImplTaskMappingDTO> getsearchtask([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.getsearchtask(data);
        }
        [Route("getcount")]
        public Task<ISM_ImplTaskMappingDTO> getcount([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.getcount(data);
        }
        [Route("deactive")]
        public ISM_ImplTaskMappingDTO deactive([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.deactive(data);
        }
        [Route("gettaskAttachment")]
        public ISM_ImplTaskMappingDTO gettaskAttachment([FromBody]ISM_ImplTaskMappingDTO data)
        {
            return _objinter.gettaskAttachment(data);
        }



    }
}
