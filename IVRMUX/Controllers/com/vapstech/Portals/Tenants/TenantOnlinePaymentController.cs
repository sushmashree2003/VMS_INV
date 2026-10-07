using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Tenants;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Tenants
{
    [Route("api/[controller]")]
    public class TenantOnlinePaymentController : Controller
    {
        TenantOnlinePaymentDelegate sdd = new TenantOnlinePaymentDelegate();
        [Route("getalldetails")]

        public TenantDashboardDTO getalldetails([FromBody] TenantDashboardDTO data)
        {
            //TenantDashboardDTO data = new TenantDashboardDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getalldetails(data);
        }

        //
        [Route("onchangetype")]

        public TenantDashboardDTO onchangetype([FromBody] TenantDashboardDTO data)
        {
            //TenantDashboardDTO data = new TenantDashboardDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.onchangetype(data);
        }

        [Route("getamountdetails")]

        public TenantDashboardDTO getamountdetails([FromBody]TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            //   data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            //if (data.Tenant_flag == "Tenant")
            //{
            //    data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            //}
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sdd.getamountdetails(data);
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

        
        [Route("generatehashsequence")]
        public TenantDashboardDTO generatehash([FromBody] TenantDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (data.ASMAY_Id == 0)
            {
                data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return sdd.generatehashseq(data);
        }


        [Route("Razorpaypaymentresponse/")]
        public ActionResult razorpaymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            response.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            string querystring = "";
            response.IVRMOP_MIID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            response.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            string payid = response.razorpay_payment_id;
            try
            {
                dto = sdd.razorgetpaymentresponse(response);
                if (dto.status != "" && dto.status != null)
                {
                    querystring = "http://localhost:57606/#/app/TenantOnlinePayment/13?status=" + dto.status;
                }
                else
                {
                    querystring = "http://localhost:57606/#/app/TenantOnlinePayment/13?status=Networkfailure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }
            return Redirect(querystring);
        }

    }
}
