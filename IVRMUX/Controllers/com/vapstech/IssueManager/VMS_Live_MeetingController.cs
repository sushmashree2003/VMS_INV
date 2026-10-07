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
    public class VMS_Live_MeetingController : Controller
    {
        VMS_Live_MeetingDelegates LMD = new VMS_Live_MeetingDelegates();

        [HttpGet]
        [Route("Getdetails/")]

        public VMS_Live_MeetingDTO Getdetails(VMS_Live_MeetingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.Getdetails(dto);
        }

        [Route("savemeeting")]
        public VMS_Live_MeetingDTO savemeeting([FromBody]VMS_Live_MeetingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.savemeeting(dto);
        }

        [Route("getempdetails")]
        public VMS_Live_MeetingDTO getempdetails(VMS_Live_MeetingDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getempdetails(dto);
        }
        [Route("onstartmeeting")]
        public VMS_Live_MeetingDTO onstartmeeting([FromBody]VMS_Live_MeetingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.onstartmeeting(dto);
        }
        [Route("endmainmeeting")]
        public VMS_Live_MeetingDTO endmainmeeting([FromBody]VMS_Live_MeetingDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.endmainmeeting(dto);
        }

        [Route("getempdetails_join")]
        public VMS_Live_MeetingDTO getempdetails_join(VMS_Live_MeetingDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.getempdetails_join(dto);
        }

        [Route("onstartmeeting_join")]
        public VMS_Live_MeetingDTO onstartmeeting_join([FromBody] VMS_Live_MeetingDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.onstartmeeting_join(dto);
        }
         [Route("endmainmeeting_join")]
        public VMS_Live_MeetingDTO endmainmeeting_join([FromBody] VMS_Live_MeetingDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.endmainmeeting_join(dto);
        }
        [Route("GetReport")]
        public VMS_Live_MeetingDTO GetReport([FromBody] VMS_Live_MeetingDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return LMD.GetReport(dto);
        }


    }
}