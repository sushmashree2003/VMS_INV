using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Tenants
{
    public class TenantDashboardDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantDashboardDTO, TenantDashboardDTO> COMMM = new CommonDelegate<TenantDashboardDTO, TenantDashboardDTO>();

        public TenantDashboardDTO ViewTenantProfile(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantDashboardFacade/ViewTenantProfile/");
        }
        public TenantDashboardDTO getMonthlyBalance_details(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantDashboardFacade/getMonthlyBalance_details/");
        }
    }
}
