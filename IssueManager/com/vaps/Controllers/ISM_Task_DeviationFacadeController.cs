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
    public class ISM_Task_DeviationFacadeController : Controller
    {
        public ISM_Task_DeviationInterface _interface;
        public ISM_Task_DeviationFacadeController(ISM_Task_DeviationInterface _inter)
        {
            _interface = _inter;
        }
        [Route("GetDetails")]
        public ISM_Task_DeviationDTO GetDetails([FromBody] ISM_Task_DeviationDTO data)
        {
            return _interface.GetDetails(data);
        }
        [Route("getdeviationReport")]
        public Task<ISM_Task_DeviationDTO> getdeviationReport([FromBody]ISM_Task_DeviationDTO data)
        {
            return _interface.getdeviationReport(data);
        }
        [Route("DeptSelectionDetails")]
        public ISM_Task_DeviationDTO DeptSelectionDetails([FromBody] ISM_Task_DeviationDTO data)
        {
            return _interface.DeptSelectionDetails(data);
        }
        [Route("DesgSelectionDetails")]
        public ISM_Task_DeviationDTO DesgSelectionDetails([FromBody] ISM_Task_DeviationDTO data)
        {
            return _interface.DesgSelectionDetails(data);
        }

        [Route("SaveDateExtention")]
        public ISM_Task_DeviationDTO SaveDateExtention([FromBody] ISM_Task_DeviationDTO data)
        {
            return _interface.SaveDateExtention(data);
        }

        [Route("OnloadApproval")]
        public ISM_Task_DeviationDTO OnloadApproval([FromBody] ISM_Task_DeviationDTO data)
        {
            return _interface.OnloadApproval(data);
        }

        //==========Request Details Approval=================
        [Route("Getrequestdetails")]
        public ISM_Task_DeviationDTO Getrequestdetails([FromBody] ISM_Task_DeviationDTO data)
        {
            return _interface.Getrequestdetails(data);
        }
        [Route("SaveApprovalExtention")]
        public ISM_Task_DeviationDTO SaveApprovalExtention([FromBody] ISM_Task_DeviationDTO data)
        {
            return _interface.SaveApprovalExtention(data);
        }

        [Route("ViewApproveDetails")]
        public ISM_Task_DeviationDTO ViewApproveDetails([FromBody] ISM_Task_DeviationDTO data)
        {
            return _interface.ViewApproveDetails(data);
        }


       
    }
}
