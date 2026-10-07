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
    public class LaptopApplicationController : Controller
    {
        LaptopApplicationDelegate driver = new LaptopApplicationDelegate();

        [Route("loaddata/{id:int}")]
        public VMS_Laptop_Application_DTO loaddata(int id)
        {
            VMS_Laptop_Application_DTO data = new VMS_Laptop_Application_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.loaddata(data);
        }

        [Route("save")]
        public VMS_Laptop_Application_DTO save([FromBody]VMS_Laptop_Application_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.save(data);
        }

        [Route("edittab1")]
        public VMS_Laptop_Application_DTO edittab1([FromBody]VMS_Laptop_Application_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return driver.edittab1(data);
        }

        [Route("getApprovalStatus/{id:int}")]
        public VMS_Laptop_Application_DTO getApprovalStatus(int id)
        {
            VMS_Laptop_Application_DTO data = new VMS_Laptop_Application_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.getApprovalStatus(data);
        }

        [Route("get_status")]
        public VMS_Laptop_Application_DTO get_status([FromBody]VMS_Laptop_Application_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.get_status(data);
        }

        [Route("reject_status")]
        public VMS_Laptop_Application_DTO reject_status([FromBody]VMS_Laptop_Application_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.reject_status(data);
        }

        [Route("getrpt")]
        public VMS_Laptop_Application_DTO getrpt([FromBody]VMS_Laptop_Application_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.getrpt(data);
        }
    }
}
