using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_Master_ClientProject_PaymentController : Controller
    {
        ISM_Master_ClientProject_PaymentDelegate del = new ISM_Master_ClientProject_PaymentDelegate();

        [Route("loaddata/{id:int}")]
        public ISM_Master_ClientProject_PaymentDTO loaddata(int id)
        {
            ISM_Master_ClientProject_PaymentDTO data = new ISM_Master_ClientProject_PaymentDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }

        [Route("savedata")]
        public ISM_Master_ClientProject_PaymentDTO savedata([FromBody]ISM_Master_ClientProject_PaymentDTO data)
        {

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedata(data);
        }

        [Route("EditData")]
        public ISM_Master_ClientProject_PaymentDTO Editdata([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("clientDecative")]
        public ISM_Master_ClientProject_PaymentDTO clientDecative([FromBody]ISM_Master_ClientProject_PaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.clientDecative(data);
        }

        [Route("getProject")]
        public ISM_Master_ClientProject_PaymentDTO getProject([FromBody]ISM_Master_ClientProject_PaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getProject(data);
        }

        [Route("viewdetails")]
        public ISM_Master_ClientProject_PaymentDTO viewdetails([FromBody]ISM_Master_ClientProject_PaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdetails(data);
        }

        [Route("savepaymentdetailsrecord")]
        public ISM_Master_ClientProject_PaymentDTO savepaymentdetailsrecord([FromBody]ISM_Master_ClientProject_PaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savepaymentdetailsrecord(data);
        }

        // Configuration
        [Route("loaddataconfig/{id:int}")]
        public ISM_Master_ClientProject_PaymentDTO loaddataconfig(int id)
        {
            ISM_Master_ClientProject_PaymentDTO data = new ISM_Master_ClientProject_PaymentDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public ISM_Master_ClientProject_PaymentDTO savedataconfig([FromBody]ISM_Master_ClientProject_PaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport/{id:int}")]
        public ISM_Master_ClientProject_PaymentDTO loaddatareport(int id)
        {
            ISM_Master_ClientProject_PaymentDTO data = new ISM_Master_ClientProject_PaymentDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddatareport(data);
        }

        [Route("getreport")]
        public ISM_Master_ClientProject_PaymentDTO getreport([FromBody]ISM_Master_ClientProject_PaymentDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getreport(data);
        }
    }
}
