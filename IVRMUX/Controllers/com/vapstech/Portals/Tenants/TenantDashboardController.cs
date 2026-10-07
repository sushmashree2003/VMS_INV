using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Tenants
{
    [Route("api/[controller]")]
    public class TenantDashboardController : Controller
    {
        TenantDashboardDelegate sdd = new TenantDashboardDelegate();
        [Route("ViewTenantProfile")]

        public TenantDashboardDTO ViewTenantProfile(int id)
        {
            TenantDashboardDTO data = new TenantDashboardDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
         //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.ViewTenantProfile(data);
        }

        [Route("getMonthlyBalance_details")]

        public TenantDashboardDTO getMonthlyBalance_details([FromBody] TenantDashboardDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getMonthlyBalance_details(data);
        }

    }
}
