using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.Tenants
{
    [Route("api/[controller]")]
    public class TenantViewNoticesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public TenantViewNoticesDelegate _objdel = new TenantViewNoticesDelegate();

        [Route("getdetails/{id:int}")]
        public TenantAgreementDTO getdetails(int id)
        {
            TenantAgreementDTO data = new TenantAgreementDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }

        [Route("get_report")]
        public TenantAgreementDTO get_report([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_report(data);
        }

        [Route("ViewFiles")]
        public TenantAgreementDTO ViewFiles([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.ViewFiles(data);
        }
        [Route("get_NoticeBoardreport")]
        public TenantAgreementDTO get_NoticeBoardreport([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_NoticeBoardreport(data);
        }
    }
}
