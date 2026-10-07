using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Tenant.Interface;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Tenant.Facade
{
    [Route("api/[controller]")]
    public class TenantOnlinePaymentFacadeController : Controller
    {
        public TenantOnlinePaymentInterface _ads;

        public TenantOnlinePaymentFacadeController(TenantOnlinePaymentInterface adstu)
        {
            _ads = adstu;
        }

        [Route("getalldetails")]
        public TenantDashboardDTO getalldetails([FromBody] TenantDashboardDTO data)
        {
            return _ads.getalldetails(data);
        }

        [Route("onchangetype")]
        public TenantDashboardDTO onchangetype([FromBody] TenantDashboardDTO data)
        {
            return _ads.onchangetype(data);
        }


        [Route("getamountdetails")]
        public TenantDashboardDTO getamountdetails([FromBody] TenantDashboardDTO data)
        {
            return _ads.getamountdetails(data);
        }

        [Route("getamtwaterdetails")]
        public TenantDashboardDTO getamtwaterdetails([FromBody] TenantDashboardDTO data)
        {
            return _ads.getamtwaterdetails(data);
        }

        //Online Payment  
        [Route("generatehashsequence")]
        public TenantDashboardDTO generseq([FromBody] TenantDashboardDTO data)
        {
            return _ads.generatehashsequence(data);
        }
        [Route("razorgetpaymentresponse/")]
        public PaymentDetails getpaymentresponserazor([FromBody]PaymentDetails response)
        {

            return _ads.razorresponse(response);
        }
        [Route("paymentresponseeasybuzz/")]
        public PaymentDetails.easybuzz paymentresponseeasybuzz([FromBody]PaymentDetails.easybuzz response)
        {

            return _ads.paymentresponseeasybuzz(response);
        }

    }
}
