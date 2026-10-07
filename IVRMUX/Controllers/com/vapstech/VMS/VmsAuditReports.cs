using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.Sales;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class VmsAuditReports : Controller
    {
        VmsAuditReportsDelegate _delegate = new VmsAuditReportsDelegate();

        [HttpGet]
        [Route("getalldata/{id:int}")]
        public VmsAuditReportsDTO getalldata(int id)
        {
            VmsAuditReportsDTO dto = new VmsAuditReportsDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.getalldata(dto);


        }
        [HttpPost]
        [Route("TadaAllwence")]
        public VmsAuditReportsDTO TadaAllwence([FromBody]VmsAuditReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TadaAllwence(data);
        }
        [Route("savedta")]
        public VmsAuditReportsDTO savedta([FromBody]VmsAuditReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(data);
        }
        [Route("edit")]
        public VmsAuditReportsDTO edit([FromBody]VmsAuditReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edit(data);
        }
        //edit
        [Route("deactive")]

        public VmsAuditReportsDTO deactive([FromBody]VmsAuditReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
        
        [Route("Getdata")]

        public VmsAuditReportsDTO Getdata([FromBody]VmsAuditReportsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Getdata(data);
        }

    }
}
