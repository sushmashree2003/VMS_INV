using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_DailyReport_ApprovalController : Controller
    {
        ISM_DailyReport_ApprovalDelegate _deleg = new ISM_DailyReport_ApprovalDelegate();
        // GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [Route("GetDetails")]
        public ISM_DailyReport_ApprovalDTO GetDetails([FromBody] ISM_DailyReport_ApprovalDTO data)
        {            
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.GetDetails(data);
        }

        [Route("DeptSelectionDetails")]
        public ISM_DailyReport_ApprovalDTO DeptSelectionDetails([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.DeptSelectionDetails(data);
        }

        [Route("DesgSelectionDetails")]
        public ISM_DailyReport_ApprovalDTO DesgSelectionDetails([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.DesgSelectionDetails(data);
        }

        [Route("GetSavedDetails")]
        public ISM_DailyReport_ApprovalDTO GetSavedDetails([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.GetSavedDetails(data);
        }

        [Route("SaveApprovalDetails")]
        public ISM_DailyReport_ApprovalDTO SaveApprovalDetails([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.SaveApprovalDetails(data);
        }

        [Route("ViewCheckListSavedDetails")]
        public ISM_DailyReport_ApprovalDTO ViewCheckListSavedDetails([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.ViewCheckListSavedDetails(data);
        }

        [Route("viewresponse")]
        public ISM_DailyReport_ApprovalDTO viewresponse([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.viewresponse(data);
        }

        /* DR Not Sent Remrks Report Details */
        [Route("GetDRRemarksReport")]
        public ISM_DailyReport_ApprovalDTO GetDRRemarksReport([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.GetDRRemarksReport(data);
        }


        /* DR Not Approved Details */
        [Route("GetDRNotApprovedList")]
        public ISM_DailyReport_ApprovalDTO GetDRNotApprovedList([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.GetDRNotApprovedList(data);
        }


        //Daily Report Multi Approval
        [Route("GetMultiDRSavedDetails")]
        public ISM_DailyReport_ApprovalDTO GetMultiDRSavedDetails([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.GetMultiDRSavedDetails(data);
        }
        
        [Route("ViewApprovalTaskDetails")]
        public ISM_DailyReport_ApprovalDTO ViewApprovalTaskDetails([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.ViewApprovalTaskDetails(data);
        }  
        
        [Route("SaveDRMultiApprovalDetails")]
        public ISM_DailyReport_ApprovalDTO SaveDRMultiApprovalDetails([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.SaveDRMultiApprovalDetails(data);
        }    
        
        [Route("ViewOverAllComments")]
        public ISM_DailyReport_ApprovalDTO ViewOverAllComments([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.ViewOverAllComments(data);
        }

        [Route("SaveRegenerateDRApproval")]
        public ISM_DailyReport_ApprovalDTO SaveRegenerateDRApproval([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.SaveRegenerateDRApproval(data);
        }
        [Route("GetSavedDetails1")]
        public ISM_DailyReport_ApprovalDTO GetSavedDetails1([FromBody]ISM_DailyReport_ApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.GetSavedDetails1(data);
        }
    }
}
