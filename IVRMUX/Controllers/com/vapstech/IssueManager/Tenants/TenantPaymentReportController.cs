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
    public class TenantPaymentReportController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public TenantPaymentReprotDelegate _objdel = new TenantPaymentReprotDelegate();

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
        [Route("institutionchange")]
        public TenantAgreementDTO institutionchange([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.institutionchange(data);
        }
        [Route("buildingchange")]
        public TenantAgreementDTO buildingchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.buildingchange(data);
        }

        [Route("floorchange")]
        public TenantAgreementDTO floorchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.Floorchange(data);
        }
        [Route("TenantDetails")]
        public TenantAgreementDTO TenantDetails([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.TenantDetails(data);
        }

        [Route("TenantConsolidatedReport")]
        public TenantAgreementDTO TenantConsolidatedReport([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.TenantConsolidatedReport(data);
        }
        [Route("TenantMonthlyPaymentDetails")]
        public TenantAgreementDTO TenantMonthlyPaymentDetails([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.TenantMonthlyPaymentDetails(data);
        }


    }
}
