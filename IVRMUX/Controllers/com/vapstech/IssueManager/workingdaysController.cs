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
    public class workingdaysController : Controller
    {
        public workingdaysDelegate _objdel = new workingdaysDelegate();

        [Route("getdetails/{id:int}")]
        public workingdays_DTO getdetails(int id)
        {
            workingdays_DTO dTO = new workingdays_DTO();
            dTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _objdel.getdetails(dTO);
        }

        [Route("saverecord")]
        public workingdays_DTO saverecord([FromBody]workingdays_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            value.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saverecord(value);
        }

        [Route("deactiveY")]
        public workingdays_DTO deactiveY([FromBody]workingdays_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.deactiveY(value);
        }
        [Route("editrecord")]
        public workingdays_DTO editrecord([FromBody]workingdays_DTO value)
        {
            value.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.editrecord(value);
        }
        
    }
}
