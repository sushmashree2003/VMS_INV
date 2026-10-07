using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FMS
{
    [Route("api/[controller]")]
    public class Employee_Wise_VMS_FeedBackController : Controller
    {
        Employee_Wise_VMS_FeedBackDelegate EFB = new Employee_Wise_VMS_FeedBackDelegate();
        [Route("getfeedback")]
        public Employee_Wise_VMS_FeedBackDTO getfeedback([FromBody] Employee_Wise_VMS_FeedBackDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EFB.getfeedback(data);
        }
        [Route("Savedata")]
        public Employee_Wise_VMS_FeedBackDTO Savedata([FromBody] Employee_Wise_VMS_FeedBackDTO data)
        {
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EFB.Savedata(data);
        }


        [Route("GetMonthYear/{id:int}")]
        public HW_HeathCheckUpDTO GetMonthYear(int id)
        {
            HW_HeathCheckUpDTO data = new HW_HeathCheckUpDTO();
            return EFB.GetMonthYear(data);
        }

        [Route("employeefeedback")]
        public HW_HeathCheckUpDTO employeefeedback([FromBody] HW_HeathCheckUpDTO data)
        {
            //data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EFB.employeefeedback(data);
        }
        [Route("savecallcenter")]
        public HW_HeathCheckUpDTO savecallcenter([FromBody] HW_HeathCheckUpDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return EFB.savecallcenter(data);
        }

        [Route("ViewHealthCheckUpFiles")]
        public HW_HeathCheckUpDTO ViewHealthCheckUpFiles([FromBody] HW_HeathCheckUpDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return EFB.ViewHealthCheckUpFiles(data);
        }
    }
}
