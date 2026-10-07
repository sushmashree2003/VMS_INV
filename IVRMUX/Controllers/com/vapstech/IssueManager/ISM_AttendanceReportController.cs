using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_AttendanceReportController : Controller
    {
        public ISM_AttendanceReportDelegate del = new ISM_AttendanceReportDelegate();
        // GET: api/<controller>
        [Route("getdetails/{id:int}")]
        public ISM_AttendanceReportsDTO getdetails(int id)
        {
            ISM_AttendanceReportsDTO data = new ISM_AttendanceReportsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId=Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.PlannerFlag= Convert.ToString(HttpContext.Session.GetString("PlannerFlag"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
           
            return del.getdetails(data);
        }

        [Route("getplannerReport")]
        public ISM_AttendanceReportsDTO getplannerReport([FromBody]ISM_AttendanceReportsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            
            return del.getplannerReport(data);
        }
    }
}
