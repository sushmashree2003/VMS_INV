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
    public class MastersProjectController : Controller
    {


        public MastersProjectDelegate _objdel = new MastersProjectDelegate();

        [Route("getdetails/{id:int}")]
        public MastersProject_DTO getdetails(int id)
        {
            MastersProject_DTO dTO = new MastersProject_DTO();
            dTO.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _objdel.getdetails(dTO);
        }

        [Route("OnChangeInstitution")]
        public MastersProject_DTO OnChangeInstitution([FromBody]MastersProject_DTO value)
        {            
            value.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.OnChangeInstitution(value);
        }

        [Route("saverecord")]
        public MastersProject_DTO saverecord([FromBody]MastersProject_DTO value)
        {
            value.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.saverecord(value);
        }

        [Route("deactiveY")]
        public MastersProject_DTO deactiveY([FromBody]MastersProject_DTO value)
        {
            value.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _objdel.deactiveY(value);
        }


    }
}
