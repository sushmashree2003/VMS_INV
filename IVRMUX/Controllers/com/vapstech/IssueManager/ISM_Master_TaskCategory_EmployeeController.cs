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
    public class ISM_Master_TaskCategory_EmployeeController : Controller
    {
        ISM_Master_TaskCategory_EmployeeDelegate del = new ISM_Master_TaskCategory_EmployeeDelegate();
        [Route("loaddata/{id:int}")]
        public ISM_Master_TaskCategory_Employee_DTO loaddata(int id)
        {
            ISM_Master_TaskCategory_Employee_DTO data = new ISM_Master_TaskCategory_Employee_DTO();
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("save")]
        public ISM_Master_TaskCategory_Employee_DTO save([FromBody]ISM_Master_TaskCategory_Employee_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.save(data);
        }
        [Route("deactive")]
        public ISM_Master_TaskCategory_Employee_DTO deactive([FromBody] ISM_Master_TaskCategory_Employee_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.deactive(data);
        }
        [Route("EditData")]
        public ISM_Master_TaskCategory_Employee_DTO EditData([FromBody] ISM_Master_TaskCategory_Employee_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.EditData(data);
        }
        [Route("getcategory")]
        public ISM_Master_TaskCategory_Employee_DTO getcategory([FromBody] ISM_Master_TaskCategory_Employee_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getcategory(data);
        }
    }
}
