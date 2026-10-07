using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.IssueManager.Tenants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class VMS_SendNoticeController : Controller
    {
        VMS_SendNoticeDelegate _notic = new VMS_SendNoticeDelegate();
        // GET: api/values
        [Route("savedetail")]
        public VMS_SendNoticeDTO savedetail([FromBody]VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.savedetail(data);
        }

        [Route("Getdetails/{id:int}")]
        public VMS_SendNoticeDTO Getdetails(int id)
        {
            VMS_SendNoticeDTO obj = new VMS_SendNoticeDTO();
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            obj.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _notic.Getdetails(obj);
        }
        [Route("deactivate")]
        public VMS_SendNoticeDTO deactivate([FromBody]VMS_SendNoticeDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.deactivate(data);
        }
        [Route("editdetails")]
        public VMS_SendNoticeDTO editdetails([FromBody]VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.editdetails(data);
        }

        [Route("get_noticelist")]
        public VMS_SendNoticeDTO get_noticelist([FromBody]VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.get_noticelist(data);
        }


        [Route("getsection")]
        public VMS_SendNoticeDTO getsection([FromBody]VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getsection(data);
        }
        [Route("getstudent")]
        public VMS_SendNoticeDTO getstudent([FromBody]VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getstudent(data);
        }

        [Route("Deptselectiondetails")]
        public VMS_SendNoticeDTO Deptselectiondetails([FromBody]VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.Deptselectiondetails(data);
        }
        [Route("Desgselectiondetails")]
        public VMS_SendNoticeDTO Desgselectiondetails([FromBody]VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.Desgselectiondetails(data);
        }
        [Route("viewData")]
        public VMS_SendNoticeDTO viewData([FromBody]VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.viewData(data);
        }

        [Route("viewrecords")]


        public VMS_SendNoticeDTO viewrecords([FromBody]VMS_SendNoticeDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.viewrecords(data);
        }
        //[Route("sendmail")]
        //public VMS_SendNoticeDTO sendCallLettermail([FromBody] VMS_SendNoticeDTO data)
        //{
        //    data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
        //    return _notic.sendmail(data);
        //}
        [Route("gettenant")]
        public VMS_SendNoticeDTO gettenant([FromBody] VMS_SendNoticeDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _notic.gettenant(data);
        }
    }
}
