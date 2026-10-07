using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;



namespace IVRMUX.Controllers.com.vapstech.IssueManager.Tenants
{
    [Route("api/[controller]")]
    public class TenantPaidandPendinglistController : Controller
    {
        TenantPaidandPendinglistDelegate del = new TenantPaidandPendinglistDelegate();
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public TenantPaidandPendinglistDTO getalldetails(int id)
        {
            TenantPaidandPendinglistDTO dto = new TenantPaidandPendinglistDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.LogInUserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onloadgetdetails(dto);
        }



        [Route("get_report")]
        public TenantPaidandPendinglistDTO get_report([FromBody]TenantPaidandPendinglistDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.get_report(data);
        }
        [Route("sendmail")]
        public TenantPaidandPendinglistDTO sendmail([FromBody]TenantPaidandPendinglistDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.sendmail(data);
        }
    }
}
