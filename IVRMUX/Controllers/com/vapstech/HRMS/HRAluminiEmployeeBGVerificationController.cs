using System;

using IVRMUX.Delegates.com.vapstech.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;


namespace IVRMUX.Controllers.com.vapstech.HRMS
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class HRAluminiEmployeeBGVerificationController : Controller
    {
        HRAluminiEmployeeBGVerificationDelegate del = new HRAluminiEmployeeBGVerificationDelegate();
        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public HRAluminiEmployeeBGVerificationDTO getalldetails(int id)
        {
            HRAluminiEmployeeBGVerificationDTO dto = new HRAluminiEmployeeBGVerificationDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return del.onloadgetdetails(dto);
        }

        [HttpPost]
        public HRAluminiEmployeeBGVerificationDTO Post([FromBody] HRAluminiEmployeeBGVerificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }
        [Route("deactivate")]
        public HRAluminiEmployeeBGVerificationDTO deactivate([FromBody] HRAluminiEmployeeBGVerificationDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactivate(data);
        }

        [Route("sendResignationMail")]
        public HRAluminiEmployeeBGVerificationDTO sendResignationMail([FromBody] HRAluminiEmployeeBGVerificationDTO dto)
        {
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.sendResignationMail(dto);
        }

        [Route("EditData")]
        public HRAluminiEmployeeBGVerificationDTO Editdata([FromBody] HRAluminiEmployeeBGVerificationDTO data)
        {
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
    }
}