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
    public class ISM_TaskCreationFacade : Controller
    {

        public ISM_TaskCreationInterface _objinter;

        public ISM_TaskCreationFacade(ISM_TaskCreationInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_TaskCreationDTO> getdetails([FromBody] ISM_TaskCreationDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("getIEuser")]
        public Task<ISM_TaskCreationDTO> getIEuser([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.getIEuser(data);
        }
        [Route("getmodule")]
        public ISM_TaskCreationDTO getmodule([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.getmodule(data);
        }
        [Route("savedata")]
        public ISM_TaskCreationDTO savedata([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.savedata(data);
        }

        [Route("deactive")]
        public ISM_TaskCreationDTO deactive([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.deactive(data);
        }
        [Route("gettaskAttachment")]
        public ISM_TaskCreationDTO gettaskAttachment([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.gettaskAttachment(data);
        }
         [Route("get_category")]
        public ISM_TaskCreationDTO get_category([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.get_category(data);
        }


        //Task Creation For Client IN IVRM
        [Route("getdetailsClient")]
        public Task<ISM_TaskCreationDTO> getdetailsClient([FromBody] ISM_TaskCreationDTO data)
        {
            return _objinter.getdetailsClient(data);
        }
        [Route("savedataClient")]
        public ISM_TaskCreationDTO savedataClient([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.savedataClient(data);
        }
        [Route("SaveTaskDeptWise")]
        public ISM_TaskCreationDTO SaveTaskDeptWise([FromBody]ISM_TaskCreationDTO data)
        {
            return _objinter.SaveTaskDeptWise(data);
        }

    }
}
