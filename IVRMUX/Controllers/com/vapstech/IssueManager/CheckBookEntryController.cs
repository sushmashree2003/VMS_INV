using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class CheckBookEntryController : Controller
    {
        CheckBookEntryDelegate del = new CheckBookEntryDelegate();
        public DomainModelMsSqlServerContext _context;

        public CheckBookEntryController(DomainModelMsSqlServerContext _cont)
        {
            _context = _cont;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CheckBookEntryDTO loaddata(int id)
        {
            CheckBookEntryDTO data = new CheckBookEntryDTO();
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public CheckBookEntryDTO savedata([FromBody]CheckBookEntryDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.savedata(data);
        }
        //savedataSource
        [Route("savedataSource")]
        public CheckBookEntryDTO savedataSource([FromBody]CheckBookEntryDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.savedataSource(data);
        }
        [Route("EditData")]
        public CheckBookEntryDTO Editdata([FromBody] CheckBookEntryDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.Editdata(data);
        }
        [Route("clientDecative")]
        public CheckBookEntryDTO clientDecative([FromBody]CheckBookEntryDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.clientDecative(data);
        }

        [Route("getProject")]
        public CheckBookEntryDTO getProject([FromBody]CheckBookEntryDTO data)
        {

            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getProject(data);
        }

        [Route("getbom")]
        public CheckBookEntryDTO getbom([FromBody]CheckBookEntryDTO data)
        {
            
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if(data.optionflag=="OTPVeriFie")
            {
                data.MI_Id = data.MI_Id;
                var getemailid = _context.ApplicationUser.Where(a => a.Id == data.UserId).ToList();

                string genotp = "";
                HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());
                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();
                if (genotp.Length < 6)
                {
                    genotp = generate.getOTP();
                }
                else
                {
                    HttpContext.Session.SetString("EMAILOTP", genotp);
                    CommonLibrary.Email Email = new CommonLibrary.Email(_context);
                    data.EMAIL_Id = getemailid.FirstOrDefault().Email;

                    string s = Email.sendmail_checkbox(data.MI_Id, getemailid.FirstOrDefault().Email, "EMAILOTP", Convert.ToInt64(genotp));

                    data.message = "Please Check,OTP Is Sent To Your Email Id";
                }
            }
           
            return del.getbom(data);
        }
        [Route("companychange")]
        public CheckBookEntryDTO companychange([FromBody]CheckBookEntryDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.companychange(data);
        }

        [Route("viewdetails")]
        public CheckBookEntryDTO viewdetails([FromBody]CheckBookEntryDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdetails(data);
        }

        [Route("getinvoiceno")]
        public CheckBookEntryDTO getinvoiceno([FromBody]CheckBookEntryDTO data)
        {

            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getinvoiceno(data);
        }

        [Route("sms")]
        public CheckBookEntryDTO savepaymentdetailsrecord([FromBody]CheckBookEntryDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savepaymentdetailsrecord(data);
        }

        // Configuration
        [HttpGet]
        [Route("loaddataconfig/{id:int}")]
        public CheckBookEntryDTO loaddataconfig(int id)
        {
            CheckBookEntryDTO data = new CheckBookEntryDTO();
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddataconfig(data);
        }
        [HttpPost]
        [Route("savedataconfig")]
        public CheckBookEntryDTO savedataconfig([FromBody]CheckBookEntryDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedataconfig(data);
        }     
        [Route("loaddatareport")]
        public CheckBookEntryDTO loaddatareport([FromBody]CheckBookEntryDTO data)
        {                  
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddatareport(data);
        }
        [HttpPost]
        [Route("getreport")]
        public CheckBookEntryDTO getreport([FromBody]CheckBookEntryDTO data)
        {
         //   data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getreport(data);
        }
        //savedataEMI
        [HttpPost]
        [Route("savedataEMI")]
        public CheckBookEntryDTO savedataEMI([FromBody]CheckBookEntryDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.savedataEMI(data);
        }
        //paymentnotification
        [Route("paymentnotification")]
        public CheckBookEntryDTO paymentnotification([FromBody]CheckBookEntryDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.paymentnotification(data);
        }
        [Route("VerifyEmailOtpgen")]
        public async Task<CheckBookEntryDTO> VerifyEmailOtpgenAsync([FromBody] CheckBookEntryDTO mobno)
        {
            
            mobno.MI_Id = mobno.MI_Id;
            mobno.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            var EMAILOTP = HttpContext.Session.GetString("EMAILOTP");
            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFE"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 5)
            {
                mobno.message = "OTP Expired";
            }
            else
            {

                if (mobno.EMAILOTP == EMAILOTP)
                {
                    mobno.message = "Success";

                }
                else
                {
                    mobno.message = "Please Enter Correct OTP !";
                }
            }
           
            return mobno;
        }
        //
        [HttpPost]
        [Route("viewuploadflies")]
        public CheckBookEntryDTO viewuploadflies([FromBody] CheckBookEntryDTO data)
        {
            // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.viewuploadflies(data);
        }
    }
}