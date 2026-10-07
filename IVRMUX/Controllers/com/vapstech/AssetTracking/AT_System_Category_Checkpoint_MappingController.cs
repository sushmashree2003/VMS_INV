using DomainModel.Model;
using IVRMUX.Delegates.com.vapstech.AssetTracking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.AssetTracking;

namespace IVRMUX.Controllers.com.vapstech.AssetTracking
{
    [Route("api/[controller]")]
    public class AT_System_Category_Checkpoint_MappingController : Controller
    {
        AT_System_Category_Checkpoint_MappingDelegate _delegate = new AT_System_Category_Checkpoint_MappingDelegate();

        [HttpGet]
        [Route("getdetails/{id:int}")]
        public AT_System_Category_Checkpoint_MappingDTO Getdetails(int id)
        {
            AT_System_Category_Checkpoint_MappingDTO dto = new AT_System_Category_Checkpoint_MappingDTO();
            dto.MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.GetDetails(dto);
        }

        [HttpPost]
        [Route("saverecord")]
        public AT_System_Category_Checkpoint_MappingDTO Saverecord([FromBody] AT_System_Category_Checkpoint_MappingDTO dto)
        {
            dto.MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            return _delegate.SaveRecord(dto);
        }

        [HttpGet]
        [Route("getProjectClientdetails/{id:int}")]
        public Health_Entry_CheckupDTO getProjectDetails(int id)
        {
            Health_Entry_CheckupDTO dto = new Health_Entry_CheckupDTO();
            int session_MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            dto.MI_Id = session_MI_Id > 0 ? session_MI_Id : (id > 0 ? id : 2);
            dto.UserId = HttpContext.Session.GetInt32("UserId") ?? 1;
            return _delegate.getProjectDetails(dto);
        }

        [HttpPost]
        [Route("saveHealthRecord")]
        public Health_Entry_CheckupDTO SaveHealthRecord([FromBody] Health_Entry_CheckupDTO dto)
        {
            int session_MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            if (session_MI_Id > 0)
            {
                dto.MI_Id = session_MI_Id;
            }
            else if (dto.MI_Id == 0)
            {
                dto.MI_Id = 2;
            }

            int session_UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session_UserId > 0)
            {
                dto.UserId = session_UserId;
            }
            else if (dto.UserId == 0)
            {
                dto.UserId = 1;
            }

            return _delegate.SaveHealthRecord(dto);
        }

        [HttpPost]
        [Route("getcheckpoints")]
        public Health_Entry_CheckupDTO GetCheckpoints([FromBody] Health_Entry_CheckupDTO request)
        {
            int session_MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            if (session_MI_Id > 0)
            {
                request.MI_Id = session_MI_Id;
            }
            else if (request.MI_Id == 0)
            {
                request.MI_Id = 2;
            }

            int session_UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session_UserId > 0)
            {
                request.UserId = session_UserId;
            }

            return _delegate.GetCheckpoints(request);
        }

        [HttpPost]
        [Route("firstLevelApproval")]
        public Health_Entry_CheckupDTO FirstLevelApproval([FromBody] Health_Entry_CheckupDTO request)
        {
            int session_MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            if (session_MI_Id > 0)
            {
                request.MI_Id = session_MI_Id;
            }
            else if (request.MI_Id == 0)
            {
                request.MI_Id = 2;
            }

            int session_UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session_UserId > 0)
            {
                request.UserId = session_UserId;
            }

            return _delegate.FirstLevelApproval(request);
        }

        [HttpPost]
        [Route("saveApproval")]
        public Health_Entry_CheckupDTO saveApproval([FromBody] Health_Entry_CheckupDTO request)
        {
            int session_MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            if (session_MI_Id > 0)
            {
                request.MI_Id = session_MI_Id;
            }
            else if (request.MI_Id == 0)
            {
                request.MI_Id = 2;
            }

            int session_UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session_UserId > 0)
            {
                request.UserId = session_UserId;
            }

            return _delegate.saveApproval(request);
        }

        [HttpPost]
        [Route("getProjectSummaryReport")]
        public Health_Entry_CheckupDTO GetProjectSummaryReport([FromBody] Health_Entry_CheckupDTO request)
        {
            if (request == null)
            {
                request = new Health_Entry_CheckupDTO();
            }

            int session_MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            if (session_MI_Id > 0)
            {
                request.MI_Id = session_MI_Id;
            }
            else if (request.MI_Id == 0)
            {
                request.MI_Id = 2;
            }

            int session_UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session_UserId > 0)
            {
                request.UserId = session_UserId;
            }

            return _delegate.GetProjectSummaryReport(request);
        }

        [HttpPost]
        [Route("getSignOffReportDetails")]
        public Health_Entry_CheckupDTO getSignOffReportDetails([FromBody] Health_Entry_CheckupDTO request)
        {
            if (request == null)
            {
                request = new Health_Entry_CheckupDTO();
            }

            int session_MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            if (session_MI_Id > 0)
            {
                request.MI_Id = session_MI_Id;
            }
            else if (request.MI_Id == 0)
            {
                request.MI_Id = 2;
            }

            int session_UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session_UserId > 0)
            {
                request.UserId = session_UserId;
            }

            return _delegate.getSignOffReportDetails(request);
        }

        [HttpPost]
        [Route("getDetailedCheckupReport")]
        public Health_Entry_CheckupDTO GetCheckpointsForReport([FromBody] Health_Entry_CheckupDTO request)
        {
            if (request == null)
            {
                request = new Health_Entry_CheckupDTO();
            }

            int session_MI_Id = HttpContext.Session.GetInt32("Session_MI_Id") ?? 0;
            if (session_MI_Id > 0)
            {
                request.MI_Id = session_MI_Id;
            }
            else if (request.MI_Id == 0)
            {
                request.MI_Id = 2;
            }

            int session_UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session_UserId > 0)
            {
                request.UserId = session_UserId;
            }

            return _delegate.getDetailedCheckupReport(request);
        }

    }
}
