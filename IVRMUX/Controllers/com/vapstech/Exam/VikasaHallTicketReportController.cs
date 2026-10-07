using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class VikasaHallTicketReportController : Controller
    {
        private static FacadeUrl _config;

        private FacadeUrl fdu = new FacadeUrl();
        private readonly DomainModelMsSqlServerContext _context;

        public VikasaHallTicketReportController(IOptions<FacadeUrl> settings, DomainModelMsSqlServerContext context)
        {
            _context = context;
        }


        VikasaHallTicketReportDelegates _delobj = new VikasaHallTicketReportDelegates();

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5

        [HttpGet("{id}")]
        public VikasaHallTicketReportDTO getdetails(VikasaHallTicketReportDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.getdetails(data);
        }
        // POST api/values
        [HttpPost]

        [Route("onselectAcdYear")]
        public VikasaHallTicketReportDTO onselectAcdYear([FromBody]VikasaHallTicketReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectAcdYear(data);
        }

        [Route("onselectclass")]
        public VikasaHallTicketReportDTO onselectclass([FromBody]VikasaHallTicketReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectclass(data);
        }

        [Route("onselectSection")]
        public VikasaHallTicketReportDTO onselectSection([FromBody]VikasaHallTicketReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delobj.onselectSection(data);
        }

        [Route("report")]
        public VikasaHallTicketReportDTO report([FromBody]VikasaHallTicketReportDTO data)
        {
            string accountname = "";
            string accesskey = "";

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
            var datatstu = _context.IVRM_Storage_path_Details.ToList();
            if (datatstu.Count() > 0)
            {
                accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
            }

            string html = obj.getHtmlContentFromAzure(accountname, accesskey, "files/" + data.MI_Id, "HallticketReport.html", 0);
            data.htmldata = html;

            return _delobj.report(data);
        }

        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
