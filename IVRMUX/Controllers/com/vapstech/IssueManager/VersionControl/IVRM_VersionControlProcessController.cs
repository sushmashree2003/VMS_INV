using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.VersionControl;

namespace IVRMUX.Controllers.com.vapstech.IssueManager.VersionControl
{
    [Route("api/[controller]")]
    public class IVRM_VersionControlProcessController : Controller
    {
        IVRM_VersionControlProcessDelegate _objdel = new IVRM_VersionControlProcessDelegate();

        //===========Client Hosting Master
        [HttpGet]
        [Route("getdetails/{id:int}")]
        public IVRM_VersionControlProcessDTO getdetails(int id)
        {
            IVRM_VersionControlProcessDTO data = new IVRM_VersionControlProcessDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getdetails(data);
        }

        [HttpPost]
        [Route("savedata")]
        public IVRM_VersionControlProcessDTO savedata([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.savedata(data);
        }
        [HttpPost]
        [Route("editdata")]
        public IVRM_VersionControlProcessDTO editdata([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.editdata(data);
        }
        [HttpPost]
        [Route("activedeactive")]
        public IVRM_VersionControlProcessDTO activedeactive([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.activedeactive(data);
        }
        //===========Client Hosting Master END


        //=========Version Trascation

        [HttpGet]
        [Route("VTgetdetails/{id:int}")]
        public IVRM_VersionControlProcessDTO VTgetdetails(int id)
        {
            IVRM_VersionControlProcessDTO data = new IVRM_VersionControlProcessDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.VTgetdetails(data);
        }

        [HttpPost]
        [Route("VTgettasklist")]
        public IVRM_VersionControlProcessDTO VTgettasklist([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.VTgettasklist(data);
        }
        [HttpPost]
        [Route("VTsave")]
        public IVRM_VersionControlProcessDTO VTsave([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.VTsave(data);
        }
        [HttpPost]
        [Route("VTeditdata")]
        public IVRM_VersionControlProcessDTO VTeditdata([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.VTeditdata(data);
        }
        [HttpPost]
        [Route("VTactivedeactive")]
        public IVRM_VersionControlProcessDTO VTactivedeactive([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.VTactivedeactive(data);
        }


        //=========Version Trascation END


        //=========Version Trascation Report

        [HttpGet]
        [Route("ReportOnload/{id:int}")]
        public IVRM_VersionControlProcessDTO ReportOnload(int id)
        {
            IVRM_VersionControlProcessDTO data = new IVRM_VersionControlProcessDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.ReportOnload(data);
        }
        [HttpPost]
        [Route("GetReport")]
        public IVRM_VersionControlProcessDTO GetReport([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.GetReport(data);
        }

        //=========Version Trascation Report END

        //---------Get task list for approval of deployment
        [HttpPost]
        [Route("Loadthetasklist")]
        public IVRM_VersionControlProcessDTO Loadthetasklist([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.Loadthetasklist(data);
        }
        //---------Save task list for approval of deployment
        [HttpPost]
        [Route("savetasklist")]
        public IVRM_VersionControlProcessDTO savetasklist([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.savetasklist(data);
        }
        [HttpPost]
        [Route("getversiondetails")]
        public IVRM_VersionControlProcessDTO getversiondetails([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getversiondetails(data);
        }
        [HttpPost]
        [Route("savedeployment")]
        public IVRM_VersionControlProcessDTO savedeployment([FromBody]IVRM_VersionControlProcessDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.savedeployment(data);
        }
    }
}