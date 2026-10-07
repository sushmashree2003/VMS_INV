using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.Tenants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.Tenants
{
    [Route("api/[controller]")]
    public class TenantMasterNocChecklistController : Controller
    {
        TenantMasterNocChecklistDelegate _delegate = new TenantMasterNocChecklistDelegate();

        [HttpGet]
        [Route("getloaddata/{id:int}")]
        public TenantMasterNocChecklistDTO getloaddata(int id)
        {
            TenantMasterNocChecklistDTO data = new TenantMasterNocChecklistDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }

        //post
        [HttpPost]
        [Route("getamenity")]
        public TenantMasterNocChecklistDTO getamenity([FromBody] TenantMasterNocChecklistDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getamenity(data);
        }

        [Route("savedetails")]
        public TenantMasterNocChecklistDTO savedetails([FromBody] TenantMasterNocChecklistDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_User_Id"));
            return _delegate.savedetails(data);
        }
        [Route("activedeactive")]
        public TenantMasterNocChecklistDTO activedeactive([FromBody] TenantMasterNocChecklistDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.activedeactive(data);
        }



    }
}
