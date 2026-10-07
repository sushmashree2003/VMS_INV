using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class IVRM_ClassWorkController : Controller
    {
        IVRM_ClassWorkDelegats _notic = new IVRM_ClassWorkDelegats();
        // GET: api/values
        [Route("savedetail")]
        public IVRM_ClassWorkDTO savedetail([FromBody]IVRM_ClassWorkDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = mid;
            return _notic.savedetail(data);
        }

        [Route("Getdetails/{id:int}")]
        public IVRM_ClassWorkDTO Getdetails(int id)
        {
            IVRM_ClassWorkDTO obj = new IVRM_ClassWorkDTO();
            // id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            obj.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
          

            return _notic.Getdetails(obj); 
        }
        [Route("deactivate")]
        public IVRM_ClassWorkDTO deactivate([FromBody]IVRM_ClassWorkDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.deactivate(data);
        }
        [Route("get_classes")]
        public IVRM_ClassWorkDTO get_classes([FromBody]IVRM_ClassWorkDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.get_classes(data);
        }
        [Route("getsectiondata")]
        public IVRM_ClassWorkDTO getsectiondata([FromBody]IVRM_ClassWorkDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getsectiondata(data);
        }
        [Route("getsubject")]
        public IVRM_ClassWorkDTO getsubject([FromBody]IVRM_ClassWorkDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getsubject(data);
        }

        [Route("editData")]
        public IVRM_ClassWorkDTO editData([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
          
            return _notic.editData(data);
        }

    }
}
