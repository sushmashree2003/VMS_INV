using PreadmissionDTOs.com.vaps.Portals.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Tenant.Interface
{
   public interface TenantDashboardInterface
    {
        TenantDashboardDTO ViewTenantProfile(TenantDashboardDTO data);
        TenantDashboardDTO getMonthlyBalance_details(TenantDashboardDTO data);
    }

}
