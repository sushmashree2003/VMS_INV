using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.Portals.Student;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentDashboardController : Controller
    {
        StudentDashboardDelegate sdd = new StudentDashboardDelegate();

        [HttpGet]
        [Route("Getdetails")]      
        public StudentDashboardDTO Getdetails(StudentDashboardDTO sddto)
        {
           // StudentDashboardDTO sddto = new StudentDashboardDTO();
            sddto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            sddto.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            //sddto.ASMCL_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMCL_Id"));
            sddto.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            sddto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            sddto.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return sdd.Getdetails(sddto);
        }

        [Route("saveakpkfile")]
        public StudentDashboardDTO saveakpkfile([FromBody]StudentDashboardDTO data)
        {
           // StudentDashboardDTO data = new StudentDashboardDTO();

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.IVRMUL_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return sdd.saveakpkfile(data);
        }

    }
}
