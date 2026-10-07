using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.QRcode_Activity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager.QRcode_Activity;

namespace IVRMUX.Controllers.com.vapstech.IssueManager.QRcode_Activity
{
    [Route("api/[controller]")]
    public class StaffAreaActivityRecordController : Controller
    {

        StaffAreaActivityRecordDelegate _delegate = new StaffAreaActivityRecordDelegate();

        [HttpGet]

        [Route("getloaddata/{id:int}")]
        public StaffAreaActivityRecordDTO getloaddata(int id)
        {
            StaffAreaActivityRecordDTO data = new StaffAreaActivityRecordDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getloaddata(data);
        }
        
        [HttpPost]
        [Route("savedetails")]
        public StaffAreaActivityRecordDTO savedetails([FromBody] StaffAreaActivityRecordDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedetails(data);
        }
        [HttpPost]
        [Route("Activitysavedetails")]
        public StaffAreaActivityRecordDTO Activitysavedetails([FromBody] StaffAreaActivityRecordDTO data)
         {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.User_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Activitysavedetails(data);
        }


    }
}