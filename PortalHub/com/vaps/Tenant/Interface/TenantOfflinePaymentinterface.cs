using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Tenants;

namespace PortalHub.com.vaps.Tenant.Interface
{
    public interface TenantOfflinePaymentinterface
    {

        //Offline Payment

        TenantDashboardDTO getallofflinedetails(TenantDashboardDTO data);
        TenantDashboardDTO getfloor(TenantDashboardDTO data);
        TenantDashboardDTO getfinedetails(TenantDashboardDTO data);
        

        TenantDashboardDTO getroom(TenantDashboardDTO data);
        TenantDashboardDTO searchfilter(TenantDashboardDTO data);
        TenantDashboardDTO getmonthdetails(TenantDashboardDTO data);
        TenantDashboardDTO gettenantdetails(TenantDashboardDTO data);
        
        Task<TenantDashboardDTO> savedetails(TenantDashboardDTO data);
        TenantDashboardDTO GetSelectedRowDetails(TenantDashboardDTO data);
        TenantDashboardDTO onchangetype_offline(TenantDashboardDTO data);

        TenantDashboardDTO viewuploadflies(TenantDashboardDTO data);
        TenantDashboardDTO getbalncedetails(TenantDashboardDTO data);
        TenantDashboardDTO getbreakup(TenantDashboardDTO data);
        TenantDashboardDTO getMonthlyPaid_details(TenantDashboardDTO data);

        TenantDashboardDTO savefinedata(TenantDashboardDTO data);


    }
}
