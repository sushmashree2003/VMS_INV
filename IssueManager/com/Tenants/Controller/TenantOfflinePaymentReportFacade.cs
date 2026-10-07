using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class TenantOfflinePaymentReportFacade :Controller
    {
        // GET: api/<controller>
        public TenantOfflinePaymentReportInterface _interface;

        public TenantOfflinePaymentReportFacade(TenantOfflinePaymentReportInterface _inter)
        {
            _interface = _inter;
        }
 

        [Route("onloaddata")]
        public TenantOfflinePaymentReportDTO onloaddata([FromBody] TenantOfflinePaymentReportDTO data)
        {
            return _interface.onloaddata(data);
        }
        [Route("report")]
        public TenantOfflinePaymentReportDTO report([FromBody]TenantOfflinePaymentReportDTO data)
        {
            return _interface.report(data);
        }
        [Route("viewuploadflies")]
        public TenantOfflinePaymentReportDTO viewuploadflies([FromBody]TenantOfflinePaymentReportDTO data)
        {
            return _interface.viewuploadflies(data);
        }
        [Route("getpaymentreport")]
        public TenantOfflinePaymentReportDTO getpaymentreport([FromBody]TenantOfflinePaymentReportDTO data)
        {
            return _interface.getpaymentreport(data);
        }

    }
}
