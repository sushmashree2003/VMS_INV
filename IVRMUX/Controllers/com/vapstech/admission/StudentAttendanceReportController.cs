using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.admission
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentAttendanceReportController : Controller
    {
        //private static FacadeUrl _config;
        StudentAttendanceReportDelegate sad = new StudentAttendanceReportDelegate();
        //private FacadeUrl fdu = new FacadeUrl();

        [Route("getdatabyselectedtype/{id:int}")]
        public StudentAttendanceReportDTO getDataBySelectedType(int id)
        {
            return sad.getDataBySelectedType(id);
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Route("getdetails")]
        public StudentAttendanceReportDTO getdetails()
        {
            StudentAttendanceReportDTO data = new StudentAttendanceReportDTO();
            data.miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return sad.getinitialdata(data);
        }

        // POST api/values
        [HttpPost]
        [Route("getAttendetails")]
        public StudentAttendanceReportDTO getAttendetails([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getserdata(data);
        }
        [Route("getdatatype")]
        public StudentAttendanceReportDTO getdatatype([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getdatatype(data);
        }
        [Route("getreportdiv")]
        public StudentAttendanceReportDTO getreportdiv([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getreportdiv(data);
        }
        [Route("savetmpldatanew")]
        public StudentAttendanceReportDTO savetmpldatanew([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.savetmpldatanew(data);
        }
        [Route("onchangeyear")]
        public StudentAttendanceReportDTO onchangeyear([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.onchangeyear(data);
        }
        [Route("onclasschange")]
        public StudentAttendanceReportDTO onclasschange([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.onclasschange(data);
        }
        [Route("onsectionchange")]
        public StudentAttendanceReportDTO onsectionchange([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.onsectionchange(data);
        }
        [Route("getreport")]
        public StudentAttendanceReportDTO getreport([FromBody] StudentAttendanceReportDTO data)
        {
            int mi_id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.miid = mi_id;
            return sad.getreport(data);
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
