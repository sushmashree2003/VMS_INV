using DomainModel.Model;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Tenant.Interface
{
    public interface TenantOnlinePaymentInterface
    {
        TenantDashboardDTO getalldetails(TenantDashboardDTO data);
        TenantDashboardDTO onchangetype(TenantDashboardDTO data);

        TenantDashboardDTO getamountdetails(TenantDashboardDTO data);

        TenantDashboardDTO generatehashsequence(TenantDashboardDTO data);

        PaymentDetails razorresponse(PaymentDetails response);
        PaymentDetails.easybuzz paymentresponseeasybuzz(PaymentDetails.easybuzz response);
        TenantDashboardDTO getamtwaterdetails(TenantDashboardDTO data);


    }
}
