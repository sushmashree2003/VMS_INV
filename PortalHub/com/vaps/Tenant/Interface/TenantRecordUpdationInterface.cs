using PreadmissionDTOs.com.vaps.Portals.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Tenant.Interface
{
   public interface TenantRecordUpdationInterface
    {
        TenantDashboardDTO ViewTenantProfile(TenantDashboardDTO data);
        //anvesha
        TenantDashboardDTO Editrenewal(TenantDashboardDTO data);
    }

    
}
