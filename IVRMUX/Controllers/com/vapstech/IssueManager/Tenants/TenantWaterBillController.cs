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
    public class TenantWaterBillController : Controller
    {
        TenantWaterBillDelegate TMD = new TenantWaterBillDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public TenantWaterBillDTO loaddata(int id)
        {
            TenantWaterBillDTO dto = new TenantWaterBillDTO();
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.loaddata(dto);
        }
       

        [HttpPost]
        [Route("savetrainerfeedback")]
        public TenantWaterBillDTO savetrainerfeedback([FromBody] TenantWaterBillDTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return TMD.savetrainerfeedback(dto);
        }
        [Route("deactive")]
        public TenantWaterBillDTO deactive([FromBody]TenantWaterBillDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         return TMD.deactive(data);
        }
        [Route("viewroomwisewaterbill")]
        public TenantWaterBillDTO viewroomwisewaterbill([FromBody]TenantWaterBillDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return TMD.viewroomwisewaterbill(data);
        }
    }
}
