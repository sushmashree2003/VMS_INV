using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class CheckBookPrintController : Controller
    {
        CheckBookPrintDelegate del = new CheckBookPrintDelegate();
        public DomainModelMsSqlServerContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public CheckBookPrintController(DomainModelMsSqlServerContext _cont , SignInManager<ApplicationUser> ctx)
        {
            _context = _cont;
            _signInManager = ctx;
        }
        [Route("loaddata/{id:int}")]
        public CheckBookPrintDTO loaddata(int id)
        {
            CheckBookPrintDTO data = new CheckBookPrintDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            return del.loaddata(data);
        }

        
        
        [Route("getProject")]
        public CheckBookPrintDTO getProject([FromBody]CheckBookPrintDTO data)
        {
           
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getProject(data);
        }

        [Route("getbom")]
        public CheckBookPrintDTO getbom([FromBody]CheckBookPrintDTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getbom(data);
        }
        [Route("companychange")]
        public CheckBookPrintDTO companychange([FromBody]CheckBookPrintDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.companychange(data);
        }

        [Route("viewdetails")]
        public CheckBookPrintDTO viewdetails([FromBody]CheckBookPrintDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdetails(data);
        }

        [Route("getinvoiceno")]
        public CheckBookPrintDTO getinvoiceno([FromBody]CheckBookPrintDTO data)
        {
           
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getinvoiceno(data);
        }

        [Route("savepaymentdetailsrecord")]
        public CheckBookPrintDTO savepaymentdetailsrecord([FromBody]CheckBookPrintDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savepaymentdetailsrecord(data);
        }

        // Configuration
        [Route("loaddataconfig/{id:int}")]
        public CheckBookPrintDTO loaddataconfig(int id)
        {
            CheckBookPrintDTO data = new CheckBookPrintDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public CheckBookPrintDTO savedataconfig([FromBody]CheckBookPrintDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport/{id:int}")]
        public CheckBookPrintDTO loaddatareport(int id)
        {
            CheckBookPrintDTO data = new CheckBookPrintDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddatareport(data);
        }

        [Route("getreport")]
        public CheckBookPrintDTO getreport([FromBody]CheckBookPrintDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getreport(data);
        }
        [HttpPost]
        [Route("ForgotOTPForEmailval")]
        public async Task<CheckBookPrintDTO> ForgotOTPForEmailval([FromBody] CheckBookPrintDTO mobno)
        {          
            mobno.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            mobno.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            var getemailid = _context.ApplicationUser.Where(a => a.Id == mobno.UserId).ToList();
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
                mobno.EMAIL_Id = getemailid.FirstOrDefault().Email;

                string s = Email.sendmail_checkbox(mobno.MI_Id, getemailid.FirstOrDefault().Email, "EMAILOTP", Convert.ToInt64(genotp));

                mobno.message = "Please Check,OTP Is Sent To Your Email Id";
            }
            return mobno;
           
        }


        [Route("VerifyEmailOtpgen")]
        public async Task<CheckBookPrintDTO> VerifyEmailOtpgenAsync([FromBody] CheckBookPrintDTO mobno)
        {
            var EMAILOTP = HttpContext.Session.GetString("EMAILOTP");
            mobno.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            var passwordVerify = await _signInManager.PasswordSignInAsync(username, mobno.password, false, lockoutOnFailure: false);
                       
            if (passwordVerify.Succeeded)
            {
                DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFE"));
                DateTime dt2 = DateTime.Now;
                double totalminutes = (dt2 - dt1).TotalMinutes;

                if (totalminutes > 30)
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
                        mobno.message = "Wrong Enter OTP !";
                    }
                }
                HttpContext.Session.SetString("EmailOTPStatus", mobno.message);
            }
            else
            {
                mobno.message = "Password Not Matched";
            }              
            return del.savedata(mobno);             
        }
    }
}