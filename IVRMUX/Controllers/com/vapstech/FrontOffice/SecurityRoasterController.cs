using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    [Route("api/[controller]")]
    public class SecurityRoasterController : Controller
    {
        SecurityRoasterDelegate del = new SecurityRoasterDelegate();


        [Route("getalldetails")]
        public SecurityRoasterDTO getalldetails([FromBody] SecurityRoasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getalldetails(data);
        }
        [Route("getdata")]
        public SecurityRoasterDTO getdata([FromBody]SecurityRoasterDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(data);
        }
        [Route("edittab1")]
        public SecurityRoasterDTO edittab1([FromBody] SecurityRoasterDTO data)
        {

            return del.edittab1(data);
        }
        [Route("deactive")]
        public SecurityRoasterDTO deactive([FromBody]SecurityRoasterDTO data)
        {

            return del.deactive(data);
        }
        [Route("report")]
        public SecurityRoasterDTO report([FromBody]SecurityRoasterDTO data)
        {
            return del.report(data);
        }

        [Route("Binddata/{id:int}")]
        public SecurityRoasterDTO Binddata(int id)
        {
            SecurityRoasterDTO data = new SecurityRoasterDTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.Binddata(data);
        }
    }
}
