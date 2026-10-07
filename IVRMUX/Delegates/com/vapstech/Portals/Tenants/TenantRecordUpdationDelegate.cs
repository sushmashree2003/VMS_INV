using CommonLibrary;
using PreadmissionDTOs.com.vaps.Portals.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Portals.Tenants
{
    public class TenantRecordUpdationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<TenantDashboardDTO, TenantDashboardDTO> COMMM = new CommonDelegate<TenantDashboardDTO, TenantDashboardDTO>();

        public TenantDashboardDTO ViewTenantProfile(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantRecordUpdationFacade/ViewTenantProfile/");
        }
        public TenantDashboardDTO Editrenewal(TenantDashboardDTO data)
        {
            return COMMM.POSTPORTALData(data, "TenantRecordUpdationFacade/Editrenewal");
        }
    }
}
