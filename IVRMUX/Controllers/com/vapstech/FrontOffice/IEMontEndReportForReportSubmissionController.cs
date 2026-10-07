using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [Route("api/[controller]")]
    public class IEMontEndReportForReportSubmissionController : Controller
    {
        IEMontEndReportForReportSubmissionDelegate _del = new IEMontEndReportForReportSubmissionDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public IEMontEndReportForReportSubmissionDTO Onload(int n)
        {
            IEMontEndReportForReportSubmissionDTO data = new IEMontEndReportForReportSubmissionDTO();
                data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.Onload(data);
        }

        [HttpPost]
        [Route("saverecord")]
        public IEMontEndReportForReportSubmissionDTO saverecord([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.saverecord(data);
        }

        [HttpPost]
        [Route("DocView")]
        public IEMontEndReportForReportSubmissionDTO DocView([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.DocView(data);
        }

        [HttpPost]
        [Route("deactiveY")]
        public IEMontEndReportForReportSubmissionDTO deactiveY([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.deactiveY(data);
        }


        [HttpPost]
        [Route("EditData")]
        public IEMontEndReportForReportSubmissionDTO EditData([FromBody]IEMontEndReportForReportSubmissionDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.EditData(data);
        }

    }
}