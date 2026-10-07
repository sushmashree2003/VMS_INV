using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.HRMS;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MonthEndReportController : Controller
    {
        MonthEndReportDelegate del = new MonthEndReportDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public MonthEndReportDTO getalldetails(int id)
        {
            MonthEndReportDTO dto = new MonthEndReportDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("getEmployeedetailsBySelection")]
        public MonthEndReportDTO getEmployeedetailsBySelection([FromBody]MonthEndReportDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getEmployeedetailsBySelection(dto);
        }

        [HttpGet]
        [Route("getalldetailsQForm/{id:int}")]
        public MonthEndReportDTO getalldetailsQForm(int id)
        {
            MonthEndReportDTO dto = new MonthEndReportDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getalldetailsQForm(dto);
        }

        [Route("getEmployeedetailsQForm")]
        public MonthEndReportDTO getEmployeedetailsQForm([FromBody]MonthEndReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getEmployeedetailsQForm(data);
        }
        
        [Route("SearchUdetails")]
        public MonthEndReportDTO SearchUdetails([FromBody] MonthEndReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.SearchUdetails(data);
        }

        [Route("SearchFReport")]
        public MonthEndReportDTO SearchFReport([FromBody] MonthEndReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.SearchFReport(data);
        }
        
    }
}
