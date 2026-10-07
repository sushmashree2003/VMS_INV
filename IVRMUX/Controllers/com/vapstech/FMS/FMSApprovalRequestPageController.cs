using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.FMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FMS;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.FMS
{
    [Route("api/[controller]")]
    public class FMSApprovalRequestPageController : Controller
    {
        // GET: api/<controller>
        FMSApprovalRequestPageDelegate _delg = new FMSApprovalRequestPageDelegate();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        [Route("loaddata/{id:int}")]
        public FMSApprovalRequestPageDTO loaddata(int id)
        {
            FMSApprovalRequestPageDTO data = new FMSApprovalRequestPageDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.loaddata(data);
        }

        [Route("savedetails")]
        public FMSApprovalRequestPageDTO savedetails([FromBody] FMSApprovalRequestPageDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.HRME_Id = Convert.ToInt32(HttpContext.Session.GetInt32("HRME_Id"));
            return _delg.savedetails(data);
        }
        [Route("editdetails")]
        public FMSApprovalRequestPageDTO editdetails([FromBody] FMSApprovalRequestPageDTO data)
        {
            //  data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.editdetails(data);
        }
        [Route("deactive")]
        public FMSApprovalRequestPageDTO deactive([FromBody] FMSApprovalRequestPageDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.deactive(data);
        }

        [Route("viewuploadflies")]
        public FMSApprovalRequestPageDTO viewuploadflies([FromBody] FMSApprovalRequestPageDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delg.viewuploadflies(data);
        }
        [Route("Updatedetails")]
        public FMSApprovalRequestPageDTO Updatedetails([FromBody] FMSApprovalRequestPageDTO data)
        {
            
            return _delg.Updatedetails(data);
        }
    }
}
