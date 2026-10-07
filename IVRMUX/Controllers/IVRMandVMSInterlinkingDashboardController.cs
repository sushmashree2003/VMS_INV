using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.HRMS;
using PreadmissionDTOs;
using IVRMUX.Delegates;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class IVRMandVMSInterlinkingDashboardController : Controller
    {
        IVRMandVMSInterlinkingDashboardDelegates del = new IVRMandVMSInterlinkingDashboardDelegates();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public IVRMandVMSInterlinkingDashboardDTO getalldetails(int id)
        {
            IVRMandVMSInterlinkingDashboardDTO dto = new IVRMandVMSInterlinkingDashboardDTO();
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

        [Route("getclientdetails")]
        public IVRMandVMSInterlinkingDashboardDTO getclientdetails([FromBody]IVRMandVMSInterlinkingDashboardDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.getclientdetails(dto);
        }

        [HttpGet]
        [Route("getalldetailsQForm/{id:int}")]
        public IVRMandVMSInterlinkingDashboardDTO getalldetailsQForm(int id)
        {
            IVRMandVMSInterlinkingDashboardDTO dto = new IVRMandVMSInterlinkingDashboardDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getalldetailsQForm(dto);
        }

        [Route("getEmployeedetailsQForm")]
        public IVRMandVMSInterlinkingDashboardDTO getEmployeedetailsQForm([FromBody]IVRMandVMSInterlinkingDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getEmployeedetailsQForm(data);
        }

        [Route("SearchUdetails")]
        public IVRMandVMSInterlinkingDashboardDTO SearchUdetails([FromBody] IVRMandVMSInterlinkingDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.SearchUdetails(data);
        }

        [Route("SearchFReport")]
        public IVRMandVMSInterlinkingDashboardDTO SearchFReport([FromBody] IVRMandVMSInterlinkingDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.SearchFReport(data);
        }

    }
}
