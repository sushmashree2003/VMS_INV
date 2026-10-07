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
    public class TenantOfflinePaymentFacade : Controller
    {
        public TenantOfflinePaymentinterface _ads;

        public TenantOfflinePaymentFacade(TenantOfflinePaymentinterface adstu)
        {
            _ads = adstu;
        }

        [Route("getallofflinedetails")]
        public TenantDashboardDTO getallofflinedetails([FromBody] TenantDashboardDTO data)
        {
            return _ads.getallofflinedetails(data);
        }

        [Route("getfloor")]
        public TenantDashboardDTO getbuilding([FromBody] TenantDashboardDTO data)
        {
            return _ads.getfloor(data);
        }
        

        [Route("getroom")]
        public TenantDashboardDTO getroom([FromBody] TenantDashboardDTO data)
        {
            return _ads.getroom(data);
        }
        [Route("searchfilter")]
        public TenantDashboardDTO searchfilter([FromBody] TenantDashboardDTO data)
        {
            return _ads.searchfilter(data);
        }
        [Route("getmonthdetails")]
        public TenantDashboardDTO getmonthdetails([FromBody] TenantDashboardDTO data)
        {
            return _ads.getmonthdetails(data);
        }

        [Route("gettenantdetails")]
        public TenantDashboardDTO gettenantdetails([FromBody] TenantDashboardDTO data)
        {
            return _ads.gettenantdetails(data);
        }
        
        [HttpPost]
        [Route("savedetails")]
        public Task<TenantDashboardDTO> savedetails([FromBody] TenantDashboardDTO pgmodu)
        {
            return _ads.savedetails(pgmodu);
        }

        [Route("GetSelectedRowDetails")]
        public TenantDashboardDTO GetSelectedRowDetails([FromBody] TenantDashboardDTO pgmodu)
        {
            return _ads.GetSelectedRowDetails(pgmodu);
        }

        [Route("onchangetype_offline")]
        public TenantDashboardDTO onchangetype_offline([FromBody] TenantDashboardDTO data)
        {
            return _ads.onchangetype_offline(data);
        }

        [Route("viewuploadflies")]
        public TenantDashboardDTO viewuploadflies([FromBody]TenantDashboardDTO data)
        {
            return _ads.viewuploadflies(data);
        }

        [Route("getfinedetails")]
        public TenantDashboardDTO getfinedetails([FromBody] TenantDashboardDTO data)
        {
            return _ads.getfinedetails(data);
        }

        [Route("getbalncedetails")]
        public TenantDashboardDTO getbalncedetails([FromBody] TenantDashboardDTO data)
        {
            return _ads.getbalncedetails(data);
        }


        [Route("getbreakup")]
        public TenantDashboardDTO getbreakup([FromBody] TenantDashboardDTO data)
        {
            return _ads.getbreakup(data);
        }

        [Route("getMonthlyPaid_details")]
        public TenantDashboardDTO getMonthlyPaid_details([FromBody] TenantDashboardDTO data)
        {
            return _ads.getMonthlyPaid_details(data);
        }

        [Route("savefinedata")]
        public TenantDashboardDTO savefinedata([FromBody] TenantDashboardDTO data)
        {
            return _ads.savefinedata(data);
        }
    }
}
