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
    public class TenantOfflinePaymentReportController : Controller
    {
        TenantOfflinePaymentReportDelegate _Tenanat = new TenantOfflinePaymentReportDelegate();
        // GET: api/<controller>
     


        [Route("onloaddata/{id:int}")]
        public TenantOfflinePaymentReportDTO onloaddata(int id)
        {
            TenantOfflinePaymentReportDTO data = new TenantOfflinePaymentReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Tenanat.onloaddata(data);
        }

        [HttpPost]
        [Route("report")]
        public TenantOfflinePaymentReportDTO report([FromBody]TenantOfflinePaymentReportDTO data)
        {
            // TenantOfflinePaymentReportDTO data = new TenantOfflinePaymentReportDTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Tenanat.report(data);
        }
        [Route("viewuploadflies")]
        public TenantOfflinePaymentReportDTO viewuploadflies([FromBody] TenantOfflinePaymentReportDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Tenanat.viewuploadflies(d);
        }
        [Route("getpaymentreport")]
        public TenantOfflinePaymentReportDTO getpaymentreport([FromBody]TenantOfflinePaymentReportDTO data)
        {
            data.TENANTAGR_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Tenanat.getpaymentreport(data);
        }

    }
}
