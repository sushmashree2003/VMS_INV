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
    public class CheckBankBalanceController : Controller
    {
        CheckBankBalanceDelegate del = new CheckBankBalanceDelegate();

        [Route("loaddata/{id:int}")]
        public CheckBankBalanceDTO loaddata(int id)
        {
            CheckBankBalanceDTO data = new CheckBankBalanceDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public CheckBankBalanceDTO savedata([FromBody]CheckBankBalanceDTO data)
        {

         
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.savedata(data);
        }
        //GetReport
        [Route("getReporttwo")]
        public CheckBankBalanceDTO getReporttwo([FromBody]CheckBankBalanceDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
         
            return del.getReporttwo(data);
        }
        //ClearenceDate
        [Route("ClearenceDate")]
        public CheckBankBalanceDTO ClearenceDate([FromBody]CheckBankBalanceDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.ClearenceDate(data);
        }
        [Route("EditData")]
        public CheckBankBalanceDTO Editdata([FromBody] CheckBankBalanceDTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("clientDecative")]
        public CheckBankBalanceDTO clientDecative([FromBody]CheckBankBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.clientDecative(data);
        }

        [Route("getProject")]
        public VMSMaster_ChequeBookdto getProject([FromBody]VMSMaster_ChequeBookdto data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getProject(data);
        }

        [Route("getbom")]
        public CheckBankBalanceDTO getbom([FromBody]CheckBankBalanceDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getbom(data);
        }
        [Route("companychange")]
        public CheckBankBalanceDTO companychange([FromBody]CheckBankBalanceDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.companychange(data);
        }

        [Route("viewdetails")]
        public VMSMaster_ChequeBookdto viewdetails([FromBody]VMSMaster_ChequeBookdto data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdetails(data);
        }

        [Route("getinvoiceno")]
        public CheckBankBalanceDTO getinvoiceno([FromBody]CheckBankBalanceDTO data)
        {
           
          
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getinvoiceno(data);
        }

        [Route("savepaymentdetailsrecord")]
        public VMSMaster_ChequeBookdto savepaymentdetailsrecord([FromBody]VMSMaster_ChequeBookdto data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savepaymentdetailsrecord(data);
        }

        // Configuration
        [Route("loaddataconfig")]
        public VMSMaster_ChequeBookdto loaddataconfig([FromBody]VMSMaster_ChequeBookdto data)
        {
        
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public CheckBankBalanceDTO savedataconfig([FromBody]CheckBankBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedataconfig(data);
        }
        //paymentnotification
        [Route("paymentnotification")]
        public CheckBankBalanceDTO paymentnotification([FromBody]CheckBankBalanceDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.paymentnotification(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport")]
        public CheckBankBalanceDTO loaddatareport([FromBody]CheckBankBalanceDTO data)
        {
            
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddatareport(data);
        }

        [Route("getreport")]
        public CheckBankBalanceDTO getreport([FromBody]CheckBankBalanceDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getreport(data);
        }
    }
}
