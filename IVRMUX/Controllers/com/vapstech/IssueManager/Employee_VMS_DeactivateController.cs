using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class Employee_VMS_DeactivateController : Controller
    {
        Employee_VMS_DeactivateDelegate del = new Employee_VMS_DeactivateDelegate();
        [Route("Onloaddata/{id:int}")]
        public Employee_VMS_DeactivateDTO Onloaddata(int id)
        {
            Employee_VMS_DeactivateDTO data = new Employee_VMS_DeactivateDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Onloaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public Employee_VMS_DeactivateDTO savedata([FromBody]Employee_VMS_DeactivateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
    }
}