using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FMS
{
    [Route("api/[controller]")]
    public class FMS_Correspondance_ReportController : Controller
    {
        FMS_Correspondance_ReportDelegate _delg = new FMS_Correspondance_ReportDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onload/{id:int}")]
        public FMS_Correspondance_ReportDTO onload(int id)
        {
            FMS_Correspondance_ReportDTO data = new FMS_Correspondance_ReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onload(data);
        }

        [Route("onchnageradiobtnsfile")]
        public FMS_Correspondance_ReportDTO onchnageradiobtnsfile([FromBody] FMS_Correspondance_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchnageradiobtnsfile(data);
        }

        [Route("getprojectist")]
        public FMS_Correspondance_ReportDTO getprojectist([FromBody] FMS_Correspondance_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getprojectist(data);
        }

        [Route("getreport")]
        public FMS_Correspondance_ReportDTO getreport([FromBody] FMS_Correspondance_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getreport(data);
        }

        [Route("SearchReport")]
        public FMS_Correspondance_ReportDTO SearchReport([FromBody] FMS_Correspondance_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SearchReport(data);
        }

        [Route("viewreferencedetais")]
        public FMS_Correspondance_ReportDTO viewreferencedetais([FromBody] FMS_Correspondance_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.viewreferencedetais(data);
        }

        // Count Report
        [Route("CountReport")]
        public FMS_Correspondance_ReportDTO CountReport([FromBody] FMS_Correspondance_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.CountReport(data);
        }

        [Route("ViewRefernceNoDetails")]
        public FMS_Correspondance_ReportDTO ViewRefernceNoDetails([FromBody] FMS_Correspondance_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewRefernceNoDetails(data);
        }

        //File Cabinet Report
        [Route("OnloadFileCabinetReport/{id:int}")]
        public FMS_Correspondance_ReportDTO OnloadFileCabinetReport(int id)
        {
            FMS_Correspondance_ReportDTO data = new FMS_Correspondance_ReportDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnloadFileCabinetReport(data); 
        }
        [Route("FileCabinetReport")]
        public FMS_Correspondance_ReportDTO FileCabinetReport([FromBody] FMS_Correspondance_ReportDTO data)
        {            
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.FileCabinetReport(data);
        }
    }
}
