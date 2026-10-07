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
    public class DriverIndentController : Controller
    {
        DriverIndentDelegate driver = new DriverIndentDelegate();

        [Route("loaddata/{id:int}")]
        public ISM_Driver_IndentDTO loaddata(int id)
        {
            ISM_Driver_IndentDTO data = new ISM_Driver_IndentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.loaddata(data);
        }

        [Route("save")]
        public ISM_Driver_IndentDTO save([FromBody]ISM_Driver_IndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.save(data);
        }

        [Route("checkedVehicleno")]
        public ISM_Driver_IndentDTO checkedVehicleno([FromBody]ISM_Driver_IndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.checkedVehicleno(data);
        }

        

        [Route("edittab1")]
        public ISM_Driver_IndentDTO edittab1([FromBody]ISM_Driver_IndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return driver.edittab1(data);
        }

        [Route("getApprovalStatus/{id:int}")]
        public ISM_Driver_IndentDTO getApprovalStatus(int id)
        {
            ISM_Driver_IndentDTO data = new ISM_Driver_IndentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.getApprovalStatus(data);
        }

        [Route("get_status")]
        public ISM_Driver_IndentDTO get_status([FromBody]ISM_Driver_IndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.get_status(data);
        }

        [Route("reject_status")]
        public ISM_Driver_IndentDTO reject_status([FromBody]ISM_Driver_IndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.reject_status(data);
        }

        [Route("getrpt")]
        public ISM_Driver_IndentDTO getrpt([FromBody]ISM_Driver_IndentDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return driver.getrpt(data);
        }
    }
}
