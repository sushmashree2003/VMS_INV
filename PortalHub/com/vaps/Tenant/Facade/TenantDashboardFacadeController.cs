using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.Tenant.Interface;
using PreadmissionDTOs.com.vaps.Portals.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.Tenant.Facade
{
    [Route("api/[controller]")]
    public class TenantDashboardFacadeController : Controller
    {
        public TenantDashboardInterface _ads;

        public TenantDashboardFacadeController(TenantDashboardInterface adstu)
        {
            _ads = adstu;
        }

        [Route("ViewTenantProfile")]
        public TenantDashboardDTO ViewTenantProfile([FromBody] TenantDashboardDTO data)
        {
            return _ads.ViewTenantProfile(data);
        }
        [Route("getMonthlyBalance_details")]
        public TenantDashboardDTO getMonthlyBalance_details([FromBody] TenantDashboardDTO data)
        {
            return _ads.getMonthlyBalance_details(data);
        }
    }
}
