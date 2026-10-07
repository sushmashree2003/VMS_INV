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
    public class FMS_Dept_Letter_HeadMappingController : Controller
    {
        FMS_Dept_Letter_HeadMappingDelegate _delg = new FMS_Dept_Letter_HeadMappingDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("LoadData/{id:int}")]
        public FMS_Dept_Letter_HeadMappingDTO LoadData(int id)
        {
            FMS_Dept_Letter_HeadMappingDTO data = new FMS_Dept_Letter_HeadMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.LoadData(data);
        }

        [Route("OnChangeInstiution")]
        public FMS_Dept_Letter_HeadMappingDTO OnChangeInstiution([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeInstiution(data);
        }

        [Route("OnSaveData")]
        public FMS_Dept_Letter_HeadMappingDTO OnSaveData([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnSaveData(data);
        }

        [Route("EditLetterHead")]
        public FMS_Dept_Letter_HeadMappingDTO EditLetterHead([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.EditLetterHead(data);
        }

        [Route("OnDelete")]
        public FMS_Dept_Letter_HeadMappingDTO OnDelete([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnDelete(data);
        }

        // Letter Head Tracking

        [Route("TrackingLoadData/{id:int}")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingLoadData(int id)
        {
            FMS_Dept_Letter_HeadMappingDTO data = new FMS_Dept_Letter_HeadMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.TrackingLoadData(data);
        }

        [Route("TrackingOnChangeInstiution")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeInstiution([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.TrackingOnChangeInstiution(data);
        }

        [Route("TrackingOnChangeDept")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnChangeDept([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.TrackingOnChangeDept(data);
        }

        [Route("TrackingOnSaveData")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnSaveData([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.TrackingOnSaveData(data);
        }

        [Route("TrackingEditLetterHead")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingEditLetterHead([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.TrackingEditLetterHead(data);
        }

        [Route("TrackingOnDelete")]
        public FMS_Dept_Letter_HeadMappingDTO TrackingOnDelete([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        { 
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.TrackingOnDelete(data);
        }

        // Report 
        [Route("ReportLoadData/{id:int}")]
        public FMS_Dept_Letter_HeadMappingDTO ReportLoadData(int id)
        {
            FMS_Dept_Letter_HeadMappingDTO data = new FMS_Dept_Letter_HeadMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ReportLoadData(data);
        }

        [Route("ReportOnChangeInstiution")]
        public FMS_Dept_Letter_HeadMappingDTO ReportOnChangeInstiution([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ReportOnChangeInstiution(data);
        }

        [Route("ReportOnChangeDept")]
        public FMS_Dept_Letter_HeadMappingDTO ReportOnChangeDept([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ReportOnChangeDept(data);
        }

        [Route("GetLetterHeadReport")]
        public FMS_Dept_Letter_HeadMappingDTO GetLetterHeadReport([FromBody] FMS_Dept_Letter_HeadMappingDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetLetterHeadReport(data);
        }
        
    }
}