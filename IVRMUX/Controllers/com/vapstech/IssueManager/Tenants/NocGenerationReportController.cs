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
    public class NocGenerationReportController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public NocGenerationReportDelegate _objdel = new NocGenerationReportDelegate();

        [Route("getdetails/{id:int}")]
        public NocGenerationReportDTO getdetails(int id)
        {
            NocGenerationReportDTO data = new NocGenerationReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }

        [Route("get_report")]
        public NocGenerationReportDTO get_report([FromBody]NocGenerationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_report(data);
        }
        [Route("buildingchange")]
        public NocGenerationReportDTO buildingchange([FromBody] NocGenerationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.buildingchange(data);
        }

        [Route("floorchange")]
        public NocGenerationReportDTO floorchange([FromBody] NocGenerationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.Floorchange(data);
        }


        //--ELECTRICAL
        [Route("getallofflinedetails/{id:int}")]

        public NocGenerationReportDTO getallofflinedetailsGet(int id)
        {

            NocGenerationReportDTO data = new NocGenerationReportDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getallofflinedetails(data);
        }
        [Route("SaveElectricalbill")]
        public NocGenerationReportDTO SaveElectricalbill([FromBody]NocGenerationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.SaveElectricalbill(data);
        }

        [Route("deactive")]
        public NocGenerationReportDTO deactive([FromBody]NocGenerationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _objdel.deactive(data);
        }
        [Route("editdetails")]
        public NocGenerationReportDTO editdetails([FromBody]NocGenerationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _objdel.editdetails(data);
        }
        [Route("sendmail")]
        public NocGenerationReportDTO sendCallLettermail([FromBody] NocGenerationReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.sendmail(data);
        }
    }
}
