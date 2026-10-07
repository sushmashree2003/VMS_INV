using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_Task_DeviationController : Controller
    {
        ISM_Task_DeviationDelegate _deleg = new ISM_Task_DeviationDelegate();

        [Route("GetDetails")]
        public ISM_Task_DeviationDTO GetDetails([FromBody] ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.GetDetails(data);
        }
        [Route("getdeviationReport")]
        public ISM_Task_DeviationDTO getdeviationReport([FromBody]ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.getdeviationReport(data);
        }
        [Route("DeptSelectionDetails")]
        public ISM_Task_DeviationDTO DeptSelectionDetails([FromBody]ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.DeptSelectionDetails(data);
        }
        [Route("DesgSelectionDetails")]
        public ISM_Task_DeviationDTO DesgSelectionDetails([FromBody]ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            return _deleg.DesgSelectionDetails(data);
        }

        [Route("SaveDateExtention")]
        public ISM_Task_DeviationDTO SaveDateExtention([FromBody] ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _deleg.SaveDateExtention(data);
        }
       





        [Route("OnloadApproval")]
        public ISM_Task_DeviationDTO OnloadApproval([FromBody] ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _deleg.OnloadApproval(data);
        }


        //==========Request Details Approval=================
        [Route("Getrequestdetails")]
        public ISM_Task_DeviationDTO Getrequestdetails([FromBody] ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _deleg.Getrequestdetails(data);
        }

        [Route("SaveApprovalExtention")]
        public ISM_Task_DeviationDTO SaveApprovalExtention([FromBody] ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _deleg.SaveApprovalExtention(data);
        }


        [Route("ViewApproveDetails")]
        public ISM_Task_DeviationDTO ViewApproveDetails([FromBody] ISM_Task_DeviationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _deleg.ViewApproveDetails(data);
        }


       

    }
}