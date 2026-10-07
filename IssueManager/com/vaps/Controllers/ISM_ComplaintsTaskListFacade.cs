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
    public class ISM_ComplaintsTaskListFacade : Controller
    {

        public ISM_ComplaintsTaskListInterface _objinter;

        public ISM_ComplaintsTaskListFacade(ISM_ComplaintsTaskListInterface parameter)
        {
            _objinter = parameter;
        }

        [Route("getdetails")]
        public Task<ISM_ComplaintsTaskListDTO> getdetails([FromBody] ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.getdetails(data);
        }

        [Route("deptfilter")]
        public Task<ISM_ComplaintsTaskListDTO> deptfilter([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.deptfilter(data);
        }

        [Route("gettaskdetails")]
        public Task<ISM_ComplaintsTaskListDTO> gettaskdetails([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.gettaskdetails(data);
        }
        [Route("savePlanner")]
        public ISM_ComplaintsTaskListDTO savePlanner([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.savePlanner(data);
        }
         [Route("saveTransfer")]
        public ISM_ComplaintsTaskListDTO saveTransfer([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.saveTransfer(data);
        }
          [Route("saveResponse")]
        public ISM_ComplaintsTaskListDTO saveResponse([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.saveResponse(data);
        }

        [Route("saveMultiResponse")]
        public ISM_ComplaintsTaskListDTO saveMultiResponse([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.saveMultiResponse(data);
        }

        [Route("getsearchtask")]
        public Task<ISM_ComplaintsTaskListDTO> getsearchtask([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.getsearchtask(data);
        }
        [Route("getsearchtaskdetails")]
        public Task<ISM_ComplaintsTaskListDTO> getsearchtaskdetails([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.getsearchtaskdetails(data);
        }
        [Route("getcount")]
        public Task<ISM_ComplaintsTaskListDTO> getcount([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.getcount(data);
        }
        [Route("deactive")]
        public ISM_ComplaintsTaskListDTO deactive([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.deactive(data);
        }
        [Route("gettaskAttachment")]
        public ISM_ComplaintsTaskListDTO gettaskAttachment([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.gettaskAttachment(data);
        }

        //Global Search 
        [Route("GetTaskSearchDetails")]
        public ISM_ComplaintsTaskListDTO GetTaskSearchDetails([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.GetTaskSearchDetails(data);
        }
        //SaveApprovaldata
        [Route("SaveApprovaldata")]
        public ISM_ComplaintsTaskListDTO SaveApprovaldata([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.SaveApprovaldata(data);
        }
        #region DuplicateTaskClose
        [Route("getDuplicateTasksList")]
        public Task<ISM_ComplaintsTaskListDTO> getDuplicateTasksList([FromBody] ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.getDuplicateTasksList(data);
        }
        //saveDuplicateTask
        [Route("saveDuplicateTask")]
        public ISM_ComplaintsTaskListDTO saveDuplicateTask([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.saveDuplicateTask(data);
        }
        [Route("getDuplicateApprovalTasksList")]
        public Task<ISM_ComplaintsTaskListDTO> getDuplicateApprovalTasksList([FromBody] ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.getDuplicateApprovalTasksList(data);
        }
        [Route("approveOrRejectDuplicateTask")]
        public ISM_ComplaintsTaskListDTO approveOrRejectDuplicateTask([FromBody]ISM_ComplaintsTaskListDTO data)
        {
            return _objinter.approveOrRejectDuplicateTask(data);
        }
        #endregion

    }
}
