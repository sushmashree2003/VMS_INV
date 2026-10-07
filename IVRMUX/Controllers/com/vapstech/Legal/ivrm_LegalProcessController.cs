using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Legal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Legal;

namespace IVRMUX.Controllers.com.vapstech.Legal
{

    [Route("api/[controller]")]
    public class ivrm_LegalProcessController : Controller
    {
        ivrm_LegalProcessDelegate del = new ivrm_LegalProcessDelegate();


        // GET: api/values
        [HttpGet]
        [Route("onloaddata/{id:int}")]
        public Legal_Notice_RequestDTO onloaddata(int id)
        {
            Legal_Notice_RequestDTO dto = new Legal_Notice_RequestDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Onloaddata(dto);
        }

        [HttpGet]
        [Route("Onloadcourtdata/{id:int}")]
        public Legal_Notice_RequestDTO Onloadcourtdata(int id)
        {
            Legal_Notice_RequestDTO dto = new Legal_Notice_RequestDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Onloadcourtdata(dto);
        }

        [HttpGet]
        [Route("Onloadcasetype/{id:int}")]
        public Legal_Notice_RequestDTO Onloadcasetype(int id)
        {
            Legal_Notice_RequestDTO dto = new Legal_Notice_RequestDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Onloadcasetype(dto);
        }

        [HttpGet]
        [Route("Onloadcasetypemapping/{id:int}")]
        public Legal_Notice_RequestDTO Onloadcasetypemapping(int id)
        {
            Legal_Notice_RequestDTO dto = new Legal_Notice_RequestDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Onloadcasetypemapping(dto);
        }

        //[HttpGet]
        [Route("onclickloaddata1")]
        public Legal_Notice_RequestDTO onclickloaddata1([FromBody] Legal_Notice_RequestDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.onclickloaddata1(dto);
        }

        [Route("savedetail")]
        public Legal_Notice_RequestDTO savedetail([FromBody]Legal_Notice_RequestDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.savedetail(dto);
        }

        [Route("legalcourtsave")]
        public Legal_Notice_RequestDTO legalcourtsave([FromBody]Legal_Notice_RequestDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.legalcourtsave(dto);
        }

        [Route("legalcasetypesave")]
        public Legal_Notice_RequestDTO legalcasetypesave([FromBody]Legal_Notice_RequestDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.legalcasetypesave(dto);
        }

        [Route("Legalcasetypemappingsave")]
        public Legal_Notice_RequestDTO Legalcasetypemappingsave([FromBody]Legal_Notice_RequestDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.Legalcasetypemappingsave(dto);
        }
    }
}