using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class ApprovalInOutStatusController : Controller
    {
        ApprovalInOutStatusDelegate delobj = new ApprovalInOutStatusDelegate();
        // GET: api/<controller>
        [HttpGet]

        [Route("getDetails/{id:int}")]
        public ApprovalInOutStatus_DTO getDetails(int id)
        {
            ApprovalInOutStatus_DTO dto = new ApprovalInOutStatus_DTO();
            dto.MI_Id = id;
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getDetails(dto);
        }
        [Route("Edit")]
        public ApprovalInOutStatus_DTO Edit([FromBody] ApprovalInOutStatus_DTO dto)
        {
            // dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.EditDetails(dto);
        }
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/<controller>      
        [Route("saveData")]
        public ApprovalInOutStatus_DTO saveData([FromBody]ApprovalInOutStatus_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveData(data);
        }

        [Route("change_radiobutton")]
        public ApprovalInOutStatus_DTO change_radiobutton([FromBody]ApprovalInOutStatus_DTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.change_radiobutton(data);
        }
    }
}
