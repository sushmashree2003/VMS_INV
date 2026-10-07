using IVRMUX.Delegates.com.vapstech.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;

using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ViewOfDeploymentController : Controller
    {
        ViewOfDeploymentDelegate _delegate = new ViewOfDeploymentDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public ViewOfDeploymentDTO getloaddata(int id)
        {
            ViewOfDeploymentDTO data = new ViewOfDeploymentDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        [Route("GetOnchangeData")]
        public ViewOfDeploymentDTO GetOnchangeData([FromBody]ViewOfDeploymentDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
           // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
          // data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
          //  data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.GetOnchangeData(data);
        }
        [Route("gettaskslist")]
        public ViewOfDeploymentDTO gettaskslist([FromBody]ViewOfDeploymentDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            //  data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.gettaskslist(data);
        }
        
        [Route("GetOnchangeClient")]
        public ViewOfDeploymentDTO GetOnchangeClient([FromBody]ViewOfDeploymentDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            //  data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.GetOnchangeClient(data);
        }

        [Route("savedata")]
        public ViewOfDeploymentDTO savedata([FromBody]ViewOfDeploymentDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            //data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.savedata(data);
        }

        //scope report
        [Route("getReport")]
        public ViewOfDeploymentDTO getReport([FromBody]ViewOfDeploymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            //  data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.getReport(data);
        }

        //scope report
        [Route("approve_reject_scope")]
        public ViewOfDeploymentDTO approve_reject_scope([FromBody]ViewOfDeploymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            //  data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.approve_reject_scope(data);
        }
        [Route("get_scope_tasklist_for_approval")]
        public ViewOfDeploymentDTO get_scope_tasklist_for_approval([FromBody]ViewOfDeploymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            // data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            //  data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.get_scope_tasklist_for_approval(data);
        }
    }
}
