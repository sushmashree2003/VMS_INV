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
    public class TenantAgreementReportController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public TenantAgreementReportDelegate _objdel = new TenantAgreementReportDelegate();

        [Route("getdetails/{id:int}")]
        public TenantAgreementDTO getdetails(int id)
        {
            TenantAgreementDTO data = new TenantAgreementDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
 
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }

        [Route("get_report")]
        public TenantAgreementDTO get_report([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
          
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_report(data);
        }
        [Route("buildingchange")]
        public TenantAgreementDTO buildingchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.buildingchange(data);
        }
     
        [Route("floorchange")]
        public TenantAgreementDTO floorchange([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.Floorchange(data);
        }


       //--ELECTRICAL
        [Route("getallofflinedetails/{id:int}")]

        public TenantAgreementDTO getallofflinedetailsGet(int id)
        {

            TenantAgreementDTO data = new TenantAgreementDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getallofflinedetails(data);
        }
        [Route("SaveElectricalbill")]
        public TenantAgreementDTO SaveElectricalbill([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.SaveElectricalbill(data);
        }

        [Route("deactive")]
        public TenantAgreementDTO deactive([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         
            return _objdel.deactive(data);
        }
        [Route("editdetails")]
        public TenantAgreementDTO editdetails([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        
            return _objdel.editdetails(data);
        }
        [Route("sendmail")]
        public TenantAgreementDTO sendCallLettermail([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.sendmail(data);
        }
        //Sudarshan 22-02-2023
        [Route("Saveallbilltype")]
        public TenantAgreementDTO Saveallbilltype([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.Saveallbilltype(data);
        }

        // Fine Caluclation

        [Route("loadtenants/{id:int}")]
        public TenantAgreementDTO loadtenants(int id)
        {
            TenantAgreementDTO data = new TenantAgreementDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.loadtenants(data);
        }
        [Route("getTenantAmountDetails")]
        public TenantAgreementDTO getTenantAmountDetails([FromBody]TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _objdel.getTenantAmountDetails(data);
        }
        [Route("Savefine")]
        public TenantAgreementDTO Savefine([FromBody] TenantAgreementDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.Savefine(data);
        }

    }
}
