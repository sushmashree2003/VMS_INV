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
    public class ISM_RoleDeptMappingController : Controller
    {
        ISM_RoleDeptMappingDelegate del = new ISM_RoleDeptMappingDelegate();
        // GET: api/<controller>
        [Route("loaddata/{id:int}")]
        public ISM_RoleDeptMappingDTO loaddata(int id)
        {
            ISM_RoleDeptMappingDTO data = new ISM_RoleDeptMappingDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("savedata")]
        public ISM_RoleDeptMappingDTO savedata([FromBody]ISM_RoleDeptMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [Route("deptDecative")]
        public ISM_RoleDeptMappingDTO deptDecative([FromBody]ISM_RoleDeptMappingDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deptDecative(data);
        }
 }
}
