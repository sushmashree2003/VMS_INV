using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{

    [Route("api/[controller]")]
    public class VMS_Live_InterviewController : Controller
    {
        VMS_Live_InterviewDelegates LMD = new VMS_Live_InterviewDelegates();

        [HttpGet]
        [Route("Getdetails/")]

        public VMS_Live_InterviewDTO Getdetails(VMS_Live_InterviewDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.Getdetails(dto);
        }
        [HttpGet]
        [Route("GetOnloaddata/")]

        public VMS_Live_InterviewDTO GetOnloaddata(VMS_Live_InterviewDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.GetOnloaddata(dto);
        }

        [Route("savemeeting")]
        public VMS_Live_InterviewDTO savemeeting([FromBody]VMS_Live_InterviewDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.savemeeting(dto);
        }

        [Route("getempdetails")]
        public VMS_Live_InterviewDTO getempdetails(VMS_Live_InterviewDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getempdetails(dto);
        }
        [Route("onstartmeeting")]
        public VMS_Live_InterviewDTO onstartmeeting([FromBody]VMS_Live_InterviewDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.onstartmeeting(dto);
        }
        [Route("endmainmeeting")]
        public VMS_Live_InterviewDTO endmainmeeting([FromBody]VMS_Live_InterviewDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.endmainmeeting(dto);
        }

        [Route("getempdetails_join")]
        public VMS_Live_InterviewDTO getempdetails_join(VMS_Live_InterviewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getempdetails_join(dto);
        }

        [Route("onstartmeeting_join")]
        public VMS_Live_InterviewDTO onstartmeeting_join([FromBody] VMS_Live_InterviewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.onstartmeeting_join(dto);
        }
         [Route("endmainmeeting_join")]
        public VMS_Live_InterviewDTO endmainmeeting_join([FromBody] VMS_Live_InterviewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.endmainmeeting_join(dto);
        }
        [Route("GetReport")]
        public VMS_Live_InterviewDTO GetReport([FromBody] VMS_Live_InterviewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.GetReport(dto);
        }


    }
}