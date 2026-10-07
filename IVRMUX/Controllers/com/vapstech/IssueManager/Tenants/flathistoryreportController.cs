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
    public class flathistoryreportController : Controller
    {
        flathistoryreportDelegate _delg = new flathistoryreportDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //Master Type
        
        [Route("getdata/{id:int}")]
     
        public TenantAgreementDTO OnLoadRatingMasterType(int id)

        {
            TenantAgreementDTO data = new TenantAgreementDTO();

                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnLoadRatingMasterType(data);
        }

        [Route("get_report")]
        public TenantAgreementDTO get_report([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.get_report(data);
        }

        [Route("buildingchange")]
        public TenantAgreementDTO buildingchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.buildingchange(data);
        }
        [Route("roomchange")]
        public TenantAgreementDTO roomchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.roomchange(data);
        }
        //added by anvesha
        [Route("ViewHealthCheckUpFiles")]
        public TenantAgreementDTO ViewHealthCheckUpFiles([FromBody] TenantAgreementDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewHealthCheckUpFiles(data);
        }


    }
}
