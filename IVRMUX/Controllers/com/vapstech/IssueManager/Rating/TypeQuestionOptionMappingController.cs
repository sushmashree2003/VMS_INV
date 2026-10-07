using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.Rating;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.Rating;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager.Rating
{
    [Route("api/[controller]")]
    public class TypeQuestionOptionMappingController : Controller
    {
        // GET: api/<controller>
        // GET: api/<controller>

        TypeQuestionOptionMappingDelegate _delg = new TypeQuestionOptionMappingDelegate();
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }   

        [Route("OnLoadRatingMasterType")]
        public Rating_ReportDTO OnLoadRatingMasterType([FromBody] Rating_ReportDTO data)
        {
            if (data.FlagOnLoadOrOnChangeInst == "OnLoad")
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnLoadRatingMasterType(data);
        }

        [Route("getReport")]
        public Rating_ReportDTO getApprovalReport([FromBody]  Rating_ReportDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getReport(data);
        }

        [Route("getDeptReport")]
        public Rating_ReportDTO getDeptReport([FromBody]  Rating_ReportDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _delg.getDeptReport(data);
        }
    }
}
