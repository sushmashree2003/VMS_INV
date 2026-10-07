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
    public class ISM_Master_TaskGroupController : Controller
    {
        ISM_Master_TaskGroupDelegate del = new ISM_Master_TaskGroupDelegate();
        [Route("loaddata/{id:int}")]
        public ISM_Master_TaskGroupDTO loaddata(int id)
        {
            ISM_Master_TaskGroupDTO data = new ISM_Master_TaskGroupDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.loaddata(data);
        }
        [Route("savedata")]
        public ISM_Master_TaskGroupDTO savedata([FromBody]ISM_Master_TaskGroupDTO data)
        {
          
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        
        [Route("EditData")]
        public ISM_Master_TaskGroupDTO Editdata([FromBody] ISM_Master_TaskGroupDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("clientDecative")]
        public ISM_Master_TaskGroupDTO clientDecative([FromBody]ISM_Master_TaskGroupDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.clientDecative(data);
        }
        [Route("DeptDeactive")]
        public ISM_Master_TaskGroupDTO DeptDeactive([FromBody]ISM_Master_TaskGroupDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.DeptDeactive(data);
        }
        [Route("viewuploaddept")]
        public ISM_Master_TaskGroupDTO viewuploaddept([FromBody]ISM_Master_TaskGroupDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewuploaddept(data);
        }
    }
}
