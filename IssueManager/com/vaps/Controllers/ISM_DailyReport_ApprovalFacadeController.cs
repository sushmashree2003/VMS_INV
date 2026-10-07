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
    public class ISM_DailyReport_ApprovalFacadeController : Controller
    {
        public ISM_DailyReport_ApprovalInterface _interface;

        public ISM_DailyReport_ApprovalFacadeController(ISM_DailyReport_ApprovalInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetDetails")]
        public ISM_DailyReport_ApprovalDTO GetDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.GetDetails(data);
        }

        [Route("DeptSelectionDetails")]
        public ISM_DailyReport_ApprovalDTO DeptSelectionDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.DeptSelectionDetails(data);
        }

        [Route("DesgSelectionDetails")]
        public ISM_DailyReport_ApprovalDTO DesgSelectionDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.DesgSelectionDetails(data);
        }

        [Route("GetSavedDetails")]
        public ISM_DailyReport_ApprovalDTO GetSavedDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.GetSavedDetails(data);
        }

        [Route("SaveApprovalDetails")]
        public ISM_DailyReport_ApprovalDTO SaveApprovalDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.SaveApprovalDetails(data);
        }

        [Route("ViewCheckListSavedDetails")]
        public ISM_DailyReport_ApprovalDTO ViewCheckListSavedDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.ViewCheckListSavedDetails(data);
        }

        [Route("viewresponse")]
        public ISM_DailyReport_ApprovalDTO viewresponse([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.viewresponse(data);
        }

        /* DR Not Sent Remrks Report Details */
        [Route("GetDRRemarksReport")]
        public ISM_DailyReport_ApprovalDTO GetDRRemarksReport([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.GetDRRemarksReport(data);
        }

        /* DR Not Approved Details */
        [Route("GetDRNotApprovedList")]
        public ISM_DailyReport_ApprovalDTO GetDRNotApprovedList([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.GetDRNotApprovedList(data);
        }

        /* DR Multi Approved Details */
        [Route("GetMultiDRSavedDetails")]
        public ISM_DailyReport_ApprovalDTO GetMultiDRSavedDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.GetMultiDRSavedDetails(data);
        }

        [Route("ViewApprovalTaskDetails")]
        public ISM_DailyReport_ApprovalDTO ViewApprovalTaskDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.ViewApprovalTaskDetails(data);
        }

        [Route("SaveDRMultiApprovalDetails")]
        public ISM_DailyReport_ApprovalDTO SaveDRMultiApprovalDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.SaveDRMultiApprovalDetails(data);
        }

        [Route("ViewOverAllComments")]
        public ISM_DailyReport_ApprovalDTO ViewOverAllComments([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.ViewOverAllComments(data);
        }
        [Route("SaveRegenerateDRApproval")]
        public ISM_DailyReport_ApprovalDTO SaveRegenerateDRApproval([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.SaveRegenerateDRApproval(data);
        }
        [Route("GetSavedDetails1")]
        public ISM_DailyReport_ApprovalDTO GetSavedDetails1([FromBody] ISM_DailyReport_ApprovalDTO data)
        {
            return _interface.GetSavedDetails1(data);
        }
    }
}
