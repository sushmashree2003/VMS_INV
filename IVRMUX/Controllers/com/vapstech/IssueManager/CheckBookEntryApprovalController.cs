using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Produces("application/json")]
    [Route("api/CheckBookEntryApproval")]
    public class CheckBookEntryApprovalController : Controller
    {
        CheckBookEntryApprovalDelegate _delegate = new CheckBookEntryApprovalDelegate();
        public DomainModelMsSqlServerContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public CheckBookEntryApprovalController(DomainModelMsSqlServerContext _cont, SignInManager<ApplicationUser> ctx)
        {
            _context = _cont;
            _signInManager = ctx;
        }
        [HttpGet]
        [Route("getalldata/{id:int}")]
        public CheckBookEntryApprovalDTO getalldata(int id)
        {
            CheckBookEntryApprovalDTO dto = new CheckBookEntryApprovalDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }
        [HttpPost]
        [Route("TadaAllwence")]
        public CheckBookEntryApprovalDTO TadaAllwence([FromBody]CheckBookEntryApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TadaAllwence(data);
        }
        [Route("savedta")]
        public CheckBookEntryApprovalDTO savedta([FromBody]CheckBookEntryApprovalDTO data)
        {
            //data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(data);
        }
        //ApprovalData
        [Route("ApprovalData")]
        public CheckBookEntryApprovalDTO ApprovalData([FromBody]CheckBookEntryApprovalDTO data)
        {

            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.ApprovalData(data);
        }
        [Route("edit")]
        public CheckBookEntryApprovalDTO edit([FromBody]CheckBookEntryApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _delegate.edit(data);
        }
        //edit
        [Route("deactive")]

        public CheckBookEntryApprovalDTO deactive([FromBody]CheckBookEntryApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
        //Getdata
        [Route("Getdata")]

        public CheckApprovalDTO Getdata([FromBody]CheckApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Getdata(data);
        }
        [Route("VerifyEmailOtpgen")]
        public async Task<CheckBookEntryApprovalDTO> VerifyEmailOtpgenAsync([FromBody] CheckBookEntryApprovalDTO mobno)
        {
            // mobno.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            mobno.MI_Id = mobno.HRMDES_Id;
            mobno.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            var EMAILOTP = HttpContext.Session.GetString("EMAILOTP");
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
                    mobno.message = "Please Enter Correct OTP !";
                }
            }
            //var username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            //var passwordVerify = await _signInManager.PasswordSignInAsync(username, mobno.password, false, lockoutOnFailure: false);

            //if (passwordVerify.Succeeded)
            //{


            //}
            //else
            //{
            //    mobno.message = "Wrong Password";
            //}
            return mobno;
        }

        [HttpPost]
        [Route("ForgotOTPForEmailval")]
        public async Task<CheckBookEntryApprovalDTO> ForgotOTPForEmailval([FromBody] CheckBookEntryApprovalDTO mobno)
        {
            mobno.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // mobno.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            mobno.MI_Id = mobno.HRMDES_Id;
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
    }
}