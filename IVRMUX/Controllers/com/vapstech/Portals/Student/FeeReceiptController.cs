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
using PreadmissionDTOs.com.vaps.Fees;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class FeeReceiptController : Controller
    {
        FeeReceiptDelegate fdd = new FeeReceiptDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            
            return fdd.getloaddata(data);
        }

        [Route("getdetails")]
        public FeeStudentTransactionDTO getdetails([FromBody]FeeStudentTransactionDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Amst_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return fdd.getdetails(data);
        }

        [Route("getrecdetails")]
        public StudentDashboardDTO getrecdetails([FromBody]StudentDashboardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return fdd.getrecdetails(data);
        }

    }
}
