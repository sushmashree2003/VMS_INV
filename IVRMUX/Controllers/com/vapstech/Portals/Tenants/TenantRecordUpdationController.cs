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
    public class TenantRecordUpdationController : Controller
    {
        TenantRecordUpdationDelegate sdd = new TenantRecordUpdationDelegate();
        [Route("ViewTenantProfile")]

        public TenantDashboardDTO ViewTenantProfile(int id)
        {
            TenantDashboardDTO data = new TenantDashboardDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.ViewTenantProfile(data);
        }
        //anvesha
        [Route("Editrenewal")]
        public TenantDashboardDTO Editrenewal([FromBody] TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.Editrenewal(data);
        }


    }
}
