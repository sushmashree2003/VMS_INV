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
    public class VMS_Live_MeetingFacadeController : Controller
    {
        public VMS_Live_MeetingInterface _inter;
        public VMS_Live_MeetingFacadeController (VMS_Live_MeetingInterface inter)
        {
            _inter = inter;
        }

        [Route("Getdetails")]
        public VMS_Live_MeetingDTO Getdetails([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.Getdetails(dto);
        }
        [Route("savemeeting")]
        public VMS_Live_MeetingDTO savemeeting([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.savemeeting(dto);
        }
        [Route("getempdetails")]
        public VMS_Live_MeetingDTO getempdetails([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.getempdetails(dto);
        }
        [Route("onstartmeeting")]
        public VMS_Live_MeetingDTO onstartmeeting([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.onstartmeeting(dto);
        }
         [Route("endmainmeeting")]
        public VMS_Live_MeetingDTO endmainmeeting([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.endmainmeeting(dto);
        }
        [Route("getempdetails_join")]
        public VMS_Live_MeetingDTO getempdetails_join([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.getempdetails_join(dto);
        }
        [Route("onstartmeeting_join")]
        public VMS_Live_MeetingDTO onstartmeeting_join([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.onstartmeeting_join(dto);
        }
        [Route("endmainmeeting_join")]
        public VMS_Live_MeetingDTO endmainmeeting_join([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.endmainmeeting_join(dto);
        }
        [Route("GetReport")]
        public VMS_Live_MeetingDTO GetReport([FromBody]VMS_Live_MeetingDTO dto)
        {
            return _inter.GetReport(dto);
        }


    }
}