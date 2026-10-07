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
    public class ISM_MemoNoticeReportController : Controller
    {
        public ISM_MemoNoticeReportDelegate _objdel = new ISM_MemoNoticeReportDelegate();

        [Route("getdetails/{id:int}")]
        public ISM_EMPLOYEE_MEMONOTICEDTO getdetails(int id)
        {
            ISM_EMPLOYEE_MEMONOTICEDTO data = new ISM_EMPLOYEE_MEMONOTICEDTO();
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getdetails(data);
        }

        [Route("getReport")]
        public ISM_EMPLOYEE_MEMONOTICEDTO getReport([FromBody]ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getMemoNoticeReport(data);
        }

        [Route("gettaskdetails")]
        public ISM_EMPLOYEE_MEMONOTICEDTO gettaskdetails([FromBody]ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.gettaskdetails(data);
        }

        [Route("getdetailsNew/{id:int}")]
        public ISM_EMPLOYEE_MEMONOTICEDTO getdetailsNew(int id)
        {
            ISM_EMPLOYEE_MEMONOTICEDTO data = new ISM_EMPLOYEE_MEMONOTICEDTO();
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getdetailsNew(data);
        }

        [Route("DeptselectiondetailsNew")]
        public ISM_EMPLOYEE_MEMONOTICEDTO DeptselectiondetailsNew([FromBody]ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.DeptselectiondetailsNew(data);
        }
        
        [Route("DesgselectiondetailsNew")]
        public ISM_EMPLOYEE_MEMONOTICEDTO DesgselectiondetailsNew([FromBody]ISM_EMPLOYEE_MEMONOTICEDTO data)
        {
            data.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.DesgselectiondetailsNew(data);
        }

    }
}
