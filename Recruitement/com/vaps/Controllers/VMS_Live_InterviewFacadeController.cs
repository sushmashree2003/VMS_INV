using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Controllers
{
   

    [Route("api/[controller]")]
    public class VMS_Live_InterviewFacadeController : Controller
    {
        public VMS_Live_InterviewInterface _inter;
        public VMS_Live_InterviewFacadeController(VMS_Live_InterviewInterface inter)
        {
            _inter = inter;
        }

        [Route("Getdetails")]
        public VMS_Live_InterviewDTO Getdetails([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.Getdetails(dto);
        }
        [Route("GetOnloaddata")]
        public VMS_Live_InterviewDTO GetOnloaddata([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.GetOnloaddata(dto);
        }
        [Route("savemeeting")]
        public VMS_Live_InterviewDTO savemeeting([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.savemeeting(dto);
        }
        [Route("getempdetails")]
        public VMS_Live_InterviewDTO getempdetails([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.getempdetails(dto);
        }
        [Route("onstartmeeting")]
        public VMS_Live_InterviewDTO onstartmeeting([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.onstartmeeting(dto);
        }
         [Route("endmainmeeting")]
        public VMS_Live_InterviewDTO endmainmeeting([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.endmainmeeting(dto);
        }
        [Route("getempdetails_join")]
        public VMS_Live_InterviewDTO getempdetails_join([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.getempdetails_join(dto);
        }
        [Route("onstartmeeting_join")]
        public VMS_Live_InterviewDTO onstartmeeting_join([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.onstartmeeting_join(dto);
        }
        [Route("endmainmeeting_join")]
        public VMS_Live_InterviewDTO endmainmeeting_join([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.endmainmeeting_join(dto);
        }
        [Route("GetReport")]
        public VMS_Live_InterviewDTO GetReport([FromBody]VMS_Live_InterviewDTO dto)
        {
            return _inter.GetReport(dto);
        }


    }
}