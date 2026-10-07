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
    public class TenantOfflinePaymentController : Controller
    {
        TenantOnlinePaymentDelegate sdd = new TenantOnlinePaymentDelegate();
        [Route("getallofflinedetails/{id:int}")]

        public TenantDashboardDTO getallofflinedetailsGet(int id)
        {

            TenantDashboardDTO data = new TenantDashboardDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getallofflinedetails(data);
        }

        [Route("getfloor")]

        public TenantDashboardDTO getfloor([FromBody]TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getfloor(data);
        }

        [Route("getroom")]

        public TenantDashboardDTO getroom([FromBody]TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getroom(data);
        }
        [Route("searchfilter")]
        public TenantDashboardDTO searchfilter([FromBody] TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sdd.searchfilter(data);
        }

        [Route("getmonthdetails")]
        public TenantDashboardDTO getmonthdetails([FromBody] TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sdd.getmonthdetails(data);
        }


        [Route("gettenantdetails")]
        public TenantDashboardDTO gettenantdetails([FromBody] TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sdd.gettenantdetails(data);
        }


        
        [HttpPost]
        [Route("savedata")]
        public TenantDashboardDTO savedata([FromBody] TenantDashboardDTO data)
        {
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));


            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.User_Id = UserId;


       
            return sdd.savedetails(data);
        }
        [Route("GetSelectedRowdetails/{id:int}")]
        public TenantDashboardDTO GetSelectedRowDetails(int ID)
        {
            TenantDashboardDTO data = new TenantDashboardDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.TENANTAGRPAY_Id = ID;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.User_Id = UserId;

            return sdd.GetSelectedRowDetails(data);
        }



         [Route("getamtwaterdetails")]

        public TenantDashboardDTO getamtwaterdetails([FromBody]TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getamtwaterdetails(data);
        }


        [Route("onchangetype_offline")]

        public TenantDashboardDTO onchangetype_offline([FromBody] TenantDashboardDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.onchangetype_offline(data);
        }

        [Route("viewuploadflies")]
        public TenantDashboardDTO viewuploadflies([FromBody] TenantDashboardDTO d)
        {
            d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return sdd.viewuploadflies(d);
        }
        //
        [Route("getfine")]

        public TenantDashboardDTO getfine([FromBody]TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getfinedetails(data);
        }

        [Route("getbalnce")]

        public TenantDashboardDTO getbalnce([FromBody]TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getbalnce(data);
        }

        [Route("getbreakup")]

        public TenantDashboardDTO getbreakup([FromBody]TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getbreakup(data);
        }


        [Route("getMonthlyPaid_details")]
        public TenantDashboardDTO getMonthlyPaid_details([FromBody] TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return sdd.getMonthlyPaid_details(data);
        }

        // Sudarshan 29-07-2022

        [Route("savefinedata")]
        public TenantDashboardDTO savefinedata([FromBody] TenantDashboardDTO data)
        {
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.User_Id = UserId;
            return sdd.savefinedata(data);
        }

    }
}
