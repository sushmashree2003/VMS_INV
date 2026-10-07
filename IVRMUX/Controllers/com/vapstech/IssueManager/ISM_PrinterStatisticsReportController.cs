using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_PrinterStatisticsReportController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public ISM_PrinterStatisticsReportDelegate _objdel = new ISM_PrinterStatisticsReportDelegate();

        [Route("getdetails/{id:int}")]
        public ISM_PrinterStatisticsDTO getdetails(int id)
        {
            ISM_PrinterStatisticsDTO data = new ISM_PrinterStatisticsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }

        [Route("get_report")]
        public ISM_PrinterStatisticsDTO get_report([FromBody]ISM_PrinterStatisticsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.get_report(data);
        }
        [Route("sendmail")]
        public ISM_PrinterStatisticsDTO sendCallLettermail([FromBody] ISM_PrinterStatisticsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.sendmail(data);
        }

    }
}
