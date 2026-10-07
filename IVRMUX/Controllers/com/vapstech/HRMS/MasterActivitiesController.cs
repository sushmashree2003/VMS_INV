using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FrontOffice;
using IVRMUX.Delegates.com.vapstech.HRMS;
using IVRMUX.Delegates.com.vapstech.VMS.Training;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [Route("api/[controller]")]
    public class MasterActivitiesController : Controller
    {
        MasterActivitiesDelegate _del = new MasterActivitiesDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("onloaddata/{id:int}")]
        public MasterActivitiesDTO onloaddata(int id)
        {
            MasterActivitiesDTO data = new MasterActivitiesDTO();
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.onloaddata(data);
        }
        //[HttpPost]
        //[Route("saverecord")]
        //public MasterActivitiesDTO saverecord([FromBody] MasterActivitiesDTO data)
        //{
        //    data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
        //    data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
        //    data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return _del.saverecord(data);
        //}

        [Route("deactiveY")]
        public MasterActivitiesDTO deactiveY([FromBody] MasterActivitiesDTO data)
        {
            data.roleid = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));
            data.Userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _del.deactiveY(data);
        }
    }
}
