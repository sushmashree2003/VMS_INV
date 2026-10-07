using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Authorization;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
using System.IO;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using SendGrid;
using System.Text;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class LoginController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _accessor;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<LoginController> _logger;
        private readonly UserManager<ApplicationUser> _UserManager;

        Logindelegate pre = new Logindelegate();
        registrationdelegate registr = new registrationdelegate();
        registrationcontext _registrationcontext;
        DomainModelMsSqlServerContext _db;
        ApplicationDBContext _ApplicationDBContext;

        public LoginController(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> UserManager, DomainModelMsSqlServerContext context, ILogger<LoginController> log)
        {
            _hostingEnvironment = hostingEnvironment;
            _accessor = httpContextAccessor;
            _context = context;
            _UserManager = UserManager;
            _logger = log;
        }

        [HttpGet]

        [Route("getMIdata")]
        public LoginDTO getMIdata(regis reg)
        {
            //Removing Session value when user logs in.
            //Added on 05-03-2018
            HttpContext.Session.Remove("Session_Trip_IsOnline");
            HttpContext.Session.Remove("Session_Trip_MI_Id");
            HttpContext.Session.Remove("Session_Trip_ASMAY_Id");

            LoginDTO lgnDto = pre.getMIdata(reg);
            return lgnDto;
        }

        [Route("getMIdataMaster/{id:int}")]
        public LoginDTO getMIdataMaster(int id)
        {
            regis reg = new regis();
            reg.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            LoginDTO lgnDto = pre.getMIdataMaster(reg);
            return lgnDto;
        }
        [HttpPost]

        [Route("Regis")]
        public string regis(FileDescriptionDTO regdata)//[FromBody]
        {
            regis reg = new regis();
            var EMAILOTPSTATUS = HttpContext.Session.GetString("EmailOTPStatus");
            var MOBILEOTPSTATUS = HttpContext.Session.GetString("MobileOTPStatus");
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));
            CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);
            CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
            reg.MI_Id = cmnds.IVRM_MI_Id;
            int asmayid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            reg.ASMAY_Id = asmayid;
            // reg.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            reg.Entry_Date = DateTime.Now;
            reg.Machine_Ip_Address = remoteIpAddress.ToString();
            reg.Email_id = regdata.Email_id;
            reg.Mobileno = regdata.Mobileno;
            reg.password = regdata.Password;
            reg.username = regdata.Username;
            reg.admittedyer = regdata.admittedyer;
            reg.leftclassid = regdata.leftclassid;
            reg.classidadmitted = regdata.classidadmitted;
            reg.leftyear = regdata.leftyear;
            reg.Name = regdata.Name;
            reg.Special = regdata.Special;
            reg.CreatedDate = DateTime.Now;
            reg.UpdatedDate = DateTime.Now;
            reg.Alumni = regdata.Alumni;
            reg.courseadmittted = regdata.courseadmittted;
            reg.courseleft = regdata.courseleft;
            reg.branchadmittted = regdata.branchadmittted;
            reg.branchleft = regdata.branchleft;
            reg.semadmittted = regdata.semadmittted;
            reg.semleft = regdata.semleft;
            if (regdata.onlinepreadminfilepath != null)
            {
                reg.profile_pic_path = regdata.onlinepreadminfilepath;
            }
            else
            {
                reg.profile_pic_path = "images\\uploads\\Profile_Pics\\a_1c6e77cd-1ba4-45eb-959a-19b80a5c0dfc.jpg";
            }

            //Date:15-12-2016 Email Confirmation.

            //reg = registr.paymentPart(reg);
            reg = registr.regdata(reg);
            if (reg.returnMsg == "created")
            {
                string confirmationLink = Url.Action("ConfirmEmail", "Login", new
                {
                    userid = reg.User_Id,
                    token = reg.token
                }, protocol: HttpContext.Request.Scheme);
                reg.confirmationLink = confirmationLink;
                // registr.sendMail(reg);


                reg.MI_Id = cmnds.IVRM_MI_Id;

                //var acd_Id = _context.AcademicYear.Where(t => t.MI_Id.Equals(reg.MI_Id) && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                mstConfig = _context.mstConfig.Where(d => d.MI_Id.Equals(reg.MI_Id) && d.ASMAY_Id.Equals(Acdemic_preadmission)).ToList();
                //  stu.mstConfig = mstConfig.ToArray();

                if (mstConfig.FirstOrDefault().ISPAC_RegMailFlag == 1 && (regdata.Special == "" || regdata.Special == null))
                {
                    Email Email = new Email(_context);

                    string s = Email.sendmailreg(reg.MI_Id, regdata.Email_id, "REG", reg.User_Id, reg.password);
                }
                if (mstConfig.FirstOrDefault().ISPAC_RegSMSFlag == 1 && (regdata.Special == "" || regdata.Special == null))
                {

                    SMS sms = new SMS(_context);

                    string sm = sms.sendSmsreg(reg.MI_Id, Convert.ToInt64(regdata.Mobileno), "REG", reg.User_Id, reg.password).Result;
                }


                if (regdata.Special == "Special")
                {
                    Email Email = new Email(_context);

                    string s = Email.sendmail(reg.MI_Id, regdata.Email_id, "SPECIALREG", reg.User_Id);


                    SMS sms = new SMS(_context);

                    string sm = sms.sendSms(reg.MI_Id, Convert.ToInt64(regdata.Mobileno), "SPECIALREG", reg.User_Id).Result;


                }

                //SMS sms = new SMS(_context);
                string Smsmessage = "Account Created Successfully,Thank You";
                //sms.sendSms(reg.MI_Id, Convert.ToInt64(regdata.Mobileno), reg.User_Id, "Newaccount", Smsmessage);

            }
            else
            {
                // reg.returnMsg;
            }
            // Mobile And Email OTP CODE

            //}
            //  END 
            return reg.returnMsg;
        }

        [Route("paynow")]
        public FileDescriptionDTO paynow(FileDescriptionDTO regdata)//[FromBody]
        {
            regis reg = new regis();

            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

            CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

            CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
            reg.MI_Id = cmnds.IVRM_MI_Id;

            int asmayid = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            reg.ASMAY_Id = asmayid;

            // reg.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            reg.Entry_Date = DateTime.Now;

            reg.Email_id = regdata.Email_id;
            reg.Mobileno = regdata.Mobileno;
            reg.password = regdata.Password;
            reg.username = regdata.Username;
            reg.Name = regdata.Name;
            reg.CreatedDate = DateTime.Now;
            reg.UpdatedDate = DateTime.Now;




            //Date:15-12-2016 Email Confirmation.

            reg = registr.paymentPart(reg);
            //  reg = registr.regdata(reg);

            regdata.PaymentDetailsList = reg.PaymentDetailsList;
            regdata.Description = reg.returnMsg;


            return regdata;
        }

        [Route("paymentresponse/")]
        public ActionResult paymentresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            FileDescriptionDTO reg = new FileDescriptionDTO();
            reg.Email_id = response.email;
            reg.Mobileno = Convert.ToString(response.phone);
            reg.Name = response.firstname;
            reg.Username = response.udf2;
            reg.Password = response.udf4;

            int miiid = Convert.ToInt32(response.udf3);

            //var acd_Id = _context.AcademicYear.Where(t => t.MI_Id== miiid && Convert.ToDateTime(t.ASMAY_PreAdm_F_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_PreAdm_T_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

            var Acdemic_preadmission = _context.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == miiid).Select(d => d.ASMAY_Id).FirstOrDefault();


            response.udf6 = Convert.ToString(Acdemic_preadmission);

            LoginDTO log = new LoginDTO();

            log = getsubdomain(log);
            var sub = log.subDomainName;
            string querystring = "";
            try
            {

                if (response.status == "success")
                {
                    reg.Description = regis(reg);
                    dto = pre.getpaymentresponse(response);
                    if (reg.Description == "created")
                    {
                        querystring = "http://localhost:57606/#/login/" + sub + "?status=" + reg.Description;
                    }
                    else if (reg.Description == "Username Already exist .. !!")
                    {
                        querystring = "http://localhost:57606/#/login/" + sub + "?status=" + reg.Description;
                    }
                    else
                    {
                        reg.Description = "checkadmin";
                        querystring = "http://localhost:57606/#/login/" + sub + "?status=" + reg.Description;
                    }
                }
                else if (response.status == "failure")
                {

                    querystring = "http://localhost:57606/#/login/" + sub + "?status=failure";
                }
            }
            catch (Exception e)
            {
                //  dto.returnvalue = "";
            }

            //return Redirect(querystring);

            return Redirect(querystring);
        }


        public LoginDTO Getdata(regis reg)  // Changed return type from string on 5-11-2016
        {
            reg.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            LoginDTO lgnDto = pre.getregdata(reg);

            if (lgnDto.roleId != 0)
            {
                // Added on 16-11-2016 to set MI Id
                HttpContext.Session.SetInt32("Session_MI_Id", Convert.ToInt32(lgnDto.MI_ID));

                // Added on 16-11-2016
                HttpContext.Session.SetInt32("RoleId", Convert.ToInt32(lgnDto.roleId));  // Changed on 5-11-2016
                                                                                         // Added on 25-11-2016
                HttpContext.Session.SetInt32("UserId", Convert.ToInt32(lgnDto.userId));

                HttpContext.Session.SetInt32("UserName", Convert.ToInt32(lgnDto.UserName));

                HttpContext.Session.SetInt32("Role", Convert.ToInt32(lgnDto.role));

                HttpContext.Session.SetInt32("ASMAY_Id", Convert.ToInt32(lgnDto.ASMAY_Id));

                HttpContext.Response.Cookies.Append("IsLogged", "true", new Microsoft.AspNetCore.Http.CookieOptions() { Path = "/", HttpOnly = false, Secure = false });
                HttpContext.Session.SetInt32("AMST_Id", Convert.ToInt32(lgnDto.AMST_Id));

                HttpContext.Session.SetInt32("Userpassword", Convert.ToInt32(reg.password));
            }
            else
            {
                HttpContext.Response.Cookies.Append("IsLogged", "false");
                //lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);
            }

            return lgnDto;  // Changed return type from string on 5-11-2016
        }

        [Route("logoutCookie")]
        public LoginDTO clearCookieData()
        {
            LoginDTO lgnDto = new LoginDTO();
            Response.Cookies.Append("IsLogged", "false");
            lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);
            return lgnDto;
        }
        // Added on 5-11-2016 to reset cookie flag to false

        [Route("VerifyUserName")]
        public LoginDTO VerifyUserName([FromBody] LoginDTO UserName)
        {

            return UserName = pre.VerifyUserName(UserName);

        }


        [Route("clearsession/{id:int}")]
        public CommonDTO clearsession(int id)
        {
            CommonDTO dto = new CommonDTO();
            int instiid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            int virtualid = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));
            dto.subDomainNamelogout = HttpContext.Session.GetString("sessionsubdomainname");
            HttpContext.Session.Clear();

            //  dto.subDomainName = subdomain;

            return dto;
        }


        [Route("setvirtauldeatilsnew")]
        public CommonDTO setVirtualDataToSessionnew([FromBody] CommonDTO dto)
        {
            int checkuser = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            if (checkuser != 0)
            {
                //int orgid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
                //getRoleoForChangeInstitute(orgid);

                dto.multiplewindowvalue = "differentlogin";
                dto.subDomainName = "0";

            }
            else
            {



                //CommonDTO dto = new CommonDTO();
                var subDomain = string.Empty;
                var host = HttpContext.Request.QueryString.ToUriComponent();
                var host1 = HttpContext.Request.QueryString;
                HttpContext.Session.SetString("session_subdomainname", "");
                HttpContext.Session.Clear();
                string checksession = HttpContext.Session.GetString("sessionsubdomainname");
                if (!string.IsNullOrWhiteSpace(host))
                {
                    subDomain = host.Split('.')[0];
                    dto.subDomainName = subDomain.Trim().ToLower();
                    HttpContext.Session.SetString("session_subdomainname", dto.subDomainName);
                    HttpContext.Session.SetString("sessionsubdomainname", dto.subDomainName);
                }

                if (dto.subDomainName != "" && dto.subDomainName != null && dto.subDomainName != "0")
                {
                    HttpContext.Session.SetString("session_subdomainname", dto.subDomainName);
                    HttpContext.Session.SetString("sessionsubdomainname", dto.subDomainName);

                    CommonDTO cmn = pre.setSessionDataForVirtualSchoolsubdomain(dto);
                    //CommonDTO cmn = pre.setSessionDataForVirtualSchool(abc);

                    if (cmn.Message != "NoINT")
                    {
                        HttpContext.Session.SetInt32("session_virtualid", Convert.ToInt32(cmn.virtualid));

                        HttpContext.Session.SetInt32("Session_MI_Id_Preadmission", cmn.IVRM_MI_Id);

                        int orgid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));


                        dto.mi_id = cmn.IVRM_MI_Id;
                        LoginDTO lgnDto = pre.getconfig(dto);
                        dto.ivrmconfiglist = lgnDto.ivrmconfiglist;
                        dto.fillinstitution = lgnDto.fillinstitution;

                        dto.EmailarrayList = lgnDto.EmailarrayList;
                        dto.PhonearrayList = lgnDto.PhonearrayList;
                        dto.payment = lgnDto.payment;
                        dto.registration = lgnDto.registration;
                        dto.allyearlist = lgnDto.allyeargetlogin;
                        dto.allclasslist = lgnDto.allclasslogin;


                        HttpContext.Session.SetInt32("otpsms", Convert.ToInt32(cmn.virtualid));


                        if (dto.IVRM_MO_Id != 0 || orgid != 0)
                        {
                            dto.multiplewindowvalue = "differentlogin";
                        }
                        else if (dto.IVRM_MO_Id == 0)
                        {
                            dto.multiplewindowvalue = "logout";
                        }
                        dto.Noint = false;
                    }
                    else
                    {
                        dto.Noint = true;
                    }


                }
                else
                {
                    if (dto.hostname != "")
                    {
                        LoginDTO lgnDto = pre.getconfigtrust(dto);
                        dto.fillinstitution = lgnDto.fillinstitution;
                        dto.EmailarrayList = lgnDto.EmailarrayList;
                        dto.PhonearrayList = lgnDto.PhonearrayList;
                        dto.Noint = false;
                    }
                }
            }


            return dto;
        }

        [Route("setvirtauldeatils/{id:int}")]
        public CommonDTO setVirtualDataToSession(int id)
        {
            CommonDTO dto = new CommonDTO();
            var subDomain = string.Empty;

            var host = HttpContext.Request.Host.Host;

            if (!string.IsNullOrWhiteSpace(host))
            {
                subDomain = host.Split('.')[0];
                dto.subDomainName = subDomain.Trim().ToLower();
            }

            dto.IVRM_MO_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MO_Id"));

            if (dto.IVRM_MO_Id != 0)
            {
                dto.multiplewindowvalue = "differentlogin";
            }

            if (true && dto.subDomainName == "")
            {
                CommonDTO cmn = pre.setSessionDataForVirtualSchool(id);
                HttpContext.Session.SetInt32("Session_MO_Id", cmn.IVRM_MO_Id);
                HttpContext.Session.SetInt32("Session_MI_Id", cmn.IVRM_MI_Id);
                _logger.LogInformation("Development");
                _logger.LogInformation(cmn.IVRM_MO_Id.ToString());
                _logger.LogInformation(cmn.IVRM_MI_Id.ToString());

                if (cmn.IVRM_MO_Id != 0)
                {
                    dto.multiplewindowvalue = "multiplevalue";
                }
            }
            else
            {
                CommonDTO cmn = pre.setSessionDataForVirtualSchoolsubdomain(dto);
                HttpContext.Session.SetInt32("Session_MO_Id", cmn.IVRM_MO_Id);
                HttpContext.Session.SetInt32("Session_MI_Id", cmn.IVRM_MI_Id);
                _logger.LogInformation("Development");
                _logger.LogInformation(cmn.IVRM_MO_Id.ToString());
                _logger.LogInformation(cmn.IVRM_MI_Id.ToString());

                if (cmn.IVRM_MO_Id != 0)
                {
                    dto.multiplewindowvalue = "multiplevalue";
                }
            }

            if (dto.IVRM_MO_Id == 0)
            {
                dto.multiplewindowvalue = "logout";
            }

            return dto;
        }

        [Route("userauth")]
        public async Task<LoginDTO> userauth([FromBody] LoginDTO LoginDTO)
        {
            int checkuser = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));           

            string emailid = "";
            if (checkuser != 0)
            {
                var userupdate = _context.ApplicationUser.Single(a => a.Id == checkuser);

                var checkemp = _context.Staff_User_Login.Where(a => a.Id == checkuser).ToList();
                if (checkemp.Count > 0)
                {
                    emailid = _context.Multiple_Email_DMO.Where(t => t.HRME_Id == checkemp.FirstOrDefault().Emp_Code && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                    if(emailid==null)
                    {
                        emailid = userupdate.NormalizedEmail;
                    }
                }
                else
                {
                    emailid = userupdate.NormalizedEmail;
                }  
                
                if(emailid!="" && emailid!=null)
                {                    
                    userupdate.TwoFactorEnabled = LoginDTO.TwoFactorEnabled;
                    _context.Update(userupdate);
                    var contactExists = _context.SaveChanges();
                }
            }

            if((LoginDTO.TwoFactorEnabled == false && emailid!= "" && emailid != null))
            {
                string[] subs = LoginDTO.logindetails.Split(',');
                
                Email Email = new Email(_context);
                string s = Email.sendmailsignin(17, emailid, "EMAILOTP2FD", 0, subs);               
            }

            LoginDTO.emailid = emailid;
            return LoginDTO;
        }

        [Route("getRole")]
        public LoginDTO getLoginRole()
        {
            CommonDTO cmn = new CommonDTO();

            cmn.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            cmn.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            cmn.roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            cmn.IVRM_MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            cmn.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));

            cmn.username = Convert.ToString(HttpContext.Session.GetString("UserName"));

            cmn.amst_id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));

            return pre.getRoleData(cmn);
        }

        [Route("getRoleoForChangeInstitute/{id:int}")]
        public LoginDTO getRoleoForChangeInstitute(int id)
        {
            LoginDTO LoginDTO = new LoginDTO();
            CommonDTO cmn = new CommonDTO();

            regis reg = new regis();
            reg.MI_Id = id;
            // Added on 16-11-2016 to set MI Id
            HttpContext.Session.SetInt32("Session_MI_Id", Convert.ToInt32(reg.MI_Id));
            reg.username = Convert.ToString(HttpContext.Session.GetString("UserName"));
            reg.password = Convert.ToString(HttpContext.Session.GetString("Userpassword"));

            var acd_Id = _context.AcademicYear.Where(t => t.MI_Id.Equals(reg.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
            //LoginDTO lgnDto = pre.getregdata(reg);
            //if (lgnDto.roleId != 0)
            //{
            HttpContext.Session.SetInt32("ASMAY_Id", Convert.ToInt32(acd_Id));
            //}
            //else
            //{
            //    HttpContext.Response.Cookies.Append("IsLogged", "false");
            //    //lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);
            //}

            return LoginDTO;
        }


        [Route("getsubdomain")]
        public LoginDTO getsubdomain(LoginDTO LoginDTO)
        {
            LoginDTO.subDomainName = HttpContext.Session.GetString("session_subdomainname");
            return LoginDTO;
        }


        [Route("sendforgotpassword")]
        public LoginDTO forgotPassword([FromBody] LoginDTO email)
        {
            LoginDTO LoginDTO = new LoginDTO();

            var EMAILOTPSTATUS = "";
            var MOBILEOTPSTATUS = "";


            if (email.ForgetPasswordSelection == "EMAIL")
            {
                EMAILOTPSTATUS = HttpContext.Session.GetString("EmailOTPStatus");

                if (EMAILOTPSTATUS == "Fail" || EMAILOTPSTATUS == null)
                {
                    email.returnMsg = "EMAIL NOT VERIFIED.!";
                }
            }

            if (email.ForgetPasswordSelection == "MOBILE")
            {
                MOBILEOTPSTATUS = HttpContext.Session.GetString("MobileOTPStatus");

                if (MOBILEOTPSTATUS == "Fail" && MOBILEOTPSTATUS == null)
                {
                    email.returnMsg = "MOBILE NOT VERIFIED.!";
                }
            }


            if (EMAILOTPSTATUS == "Success" || MOBILEOTPSTATUS == "Success")
            {
                //email.userOTP = HttpContext.Session.GetString("OTP");
                LoginDTO = pre.forgotPassword(email);
                //HttpContext.Session.GetString("session_subdomainname", dto.subDomainName);
                LoginDTO.subDomainName = HttpContext.Session.GetString("session_subdomainname");
            }

            return LoginDTO;

        }


        // send forgot password to user email added on 10-11-2016
        [Route("ConfirmEmail")]
        public ActionResult ConfirmEmail(string userid, string token)
        {
            regis reg = new regis();
            reg.User_Id = Convert.ToInt64(userid);
            reg.token = token;
            bool val = registr.ConfirmMail(reg);
            if (val == true)
            {
                // reg.emailSuccessmsg = "Email confirmed successfully!";
                return Redirect("http://localhost:57606/#/login/100");
            }
            else
            {
                //  reg.emailSuccessmsg = "Error while confirming your email!";
                return View("");
            }

        }

        [Route("getOTPForMobile")]
        public async Task<string> getOTPForMobile([FromBody] LoginDTO mobno)
        {

            ApplicationUser user = new ApplicationUser();
            user = await _UserManager.FindByEmailAsync(mobno.mobileNo);
            if (user == null || !(await _UserManager.IsEmailConfirmedAsync(user)))
            {
                string genotp = "";
                int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));
                CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

                CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
                mobno.MI_ID = cmnds.IVRM_MI_Id;

                //  mobno.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));


                HttpContext.Session.SetString("exptimeFM", DateTime.Now.ToString());
                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();


                if (genotp.Length < 6)
                {
                    genotp = generate.getOTP();
                }
                else
                {
                    HttpContext.Session.SetString("MOBILEOTP", genotp);

                    SMS sms = new SMS(_context);

                    string s = await sms.sendSms(mobno.MI_ID, Convert.ToInt64(mobno.mobileNo), "EMAILOTP", Convert.ToInt64(genotp));



                    // SMS sms = new SMS(_context);
                    // string Smsmessage = "Dear User, Your OTP Number is "+ Convert.ToInt64(genotp) + ",Thank You.";
                    //await sms.sendSms(mobno.MI_ID, Convert.ToInt64(mobno.mobileNo), mobno.userId,"MobileOtp", Smsmessage);

                    mobno.message = "Please check Your SMS, OTP Is Sent To Your Mobile Number";
                }
            }
            else
            {
                mobno.message = "Mobile Number is Already exist .. !!";
            }


            return mobno.message;
        }

        [Route("VerifymobileOtp")]
        public string VerifymobileOtp([FromBody] LoginDTO mobno)
        {

            var MOBILEOTP = HttpContext.Session.GetString("MOBILEOTP");
            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFM"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 15)
            {
                mobno.message = "OTP Expired";
            }
            else
            {
                if (mobno.MOBILEOTP == MOBILEOTP)
                {
                    mobno.message = "Success";
                }
                else
                {
                    mobno.message = "Fail";
                }
            }


            HttpContext.Session.SetString("MobileOTPStatus", mobno.message);

            return mobno.message;
        }

        [Route("getOTPForEmail")]
        public async Task<string> getOTPForEmail([FromBody] LoginDTO mobno)
        {
            ApplicationUser user = new ApplicationUser();
            user = await _UserManager.FindByEmailAsync(mobno.Email);
            if (user == null || !(await _UserManager.IsEmailConfirmedAsync(user)))
            {
                string genotp = "";

                int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

                CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

                CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
                mobno.MI_ID = cmnds.IVRM_MI_Id;


                //HttpContext.Session.SetInt32("Session_MO_Id", cmnds.IVRM_MO_Id);
                //HttpContext.Session.SetInt32("Session_MI_Id", cmnds.IVRM_MI_Id);

                // mobno.MI_ID = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();

                HttpContext.Session.SetString("exptimeFE", DateTime.Now.ToString());

                if (genotp.Length < 6)
                {
                    genotp = generate.getOTP();
                }
                else
                {
                    HttpContext.Session.SetString("EMAILOTP", genotp);
                    string[] arr = new string[] { };
                    Email Email = new Email(_context);
                    string s = Email.sendmailsignin(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp), arr);

                    //var message = new SendGrid.SendGridMessage();
                    //message.From = new MailAddress("vapstech123@gmail.com", "vaps@123");
                    //message.Subject = "E-mail Verification";
                    //    message.AddTo(mobno.Email);
                    //message.Html = "<p>Dear User, Your OTP Number is " + Convert.ToInt64(genotp) + ",Thank You.</p>";
                    //var client = new Web("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");
                    //client.DeliverAsync(message).Wait();

                    mobno.message = "Please Check Your Email,OTP Is Sent To Your Email Id";
                }
            }
            else
            {
                mobno.message = "Email id Already exist .. !!";
            }

            return mobno.message;

        }

        [Route("ForgotOTPForEmail")]
        public async Task<LoginDTO> ForgotOTPForEmail([FromBody] LoginDTO mobno)
        {
            int[] arr = { 16, 17, 20, 21, 22, 24 };
            string genotp = "";

            int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));
            int miid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);
            if (!arr.Contains(cmn.IVRM_MI_Id))
            {
                var userid = _context.ApplicationUser.Where(t => t.UserName == mobno.UserName).ToList();
                cmn.IVRM_MI_Id = _context.UserRoleWithInstituteDMO.Where(a => a.Id == userid.FirstOrDefault().Id).Select(a => Convert.ToInt32(a.MI_Id)).FirstOrDefault();
            }
            //CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
            mobno.MI_ID = cmn.IVRM_MI_Id;
            if (mobno.clickedlinkname.Equals("forgotname"))
            {
                if (mobno.usertype.Equals("Student"))
                {
                    mobno.userlist = (from t in _context.Adm_M_Student
                                      where (t.MI_Id == cmn.IVRM_MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL == "S" && t.AMST_emailId.Equals(mobno.Email))
                                      select new LoginDTO
                                      {
                                          usernametype = ((t.AMST_FirstName == null ? "" : t.AMST_FirstName.ToUpper()) + " " + (t.AMST_MiddleName == null ? "" : t.AMST_MiddleName.ToUpper()) + " " + (t.AMST_LastName == null ? "" : t.AMST_LastName.ToUpper())).Trim(),
                                          AMST_Id = t.AMST_Id
                                      }
                  ).ToArray();

                    List<Adm_M_Student> allstudents = new List<Adm_M_Student>();
                    allstudents = _context.Adm_M_Student.Where(t => t.MI_Id == cmn.IVRM_MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL == "S" && t.AMST_emailId.Equals(mobno.Email)).ToList();

                    if (allstudents.Count() > 1)
                    {
                        return mobno;
                    }
                    else
                    {
                        if (allstudents.Count() == 1)
                        {
                            List<StudentAppUserLoginDMO> Studentapplogin = new List<StudentAppUserLoginDMO>();
                            Studentapplogin = _context.StudentAppUserLoginDMO.Where(t => t.AMST_ID == allstudents.FirstOrDefault().AMST_Id).ToList();
                            if (Studentapplogin.Count() > 0)
                            {
                                ApplicationUser user = new ApplicationUser();
                                user = await _UserManager.FindByIdAsync(Studentapplogin.FirstOrDefault().STD_APP_ID.ToString());
                                if (user != null)
                                {
                                    var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                                    if (ismailConfirmed == true)
                                    {
                                        await _UserManager.RemovePasswordAsync(user);
                                        await _UserManager.AddPasswordAsync(user, "Password@123");
                                        user.UpdatedDate = DateTime.Now;
                                        var result = await _UserManager.UpdateAsync(user);
                                        if (result.Succeeded)
                                        {
                                            Email Email = new Email(_context);
                                            string s = Email.sendmail(allstudents.FirstOrDefault().MI_Id, allstudents.FirstOrDefault().AMST_emailId, "StaffUserCreation", user.Id);
                                            if (s.Equals("success"))
                                            {
                                                mobno.message = "Please Check Your Email,User Name and Password is sent";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    mobno.message = "Entered Email address is Not Exists!!";
                                }
                            }
                            else
                            {
                                mobno.message = "Username is Not Exists for this student!!";
                            }
                        }
                        else
                        {
                            mobno.message = "Username is Not Exists for this Email!!";
                        }
                    }
                }
                else
                {
                    ApplicationUser user = new ApplicationUser();
                    user = await _UserManager.FindByEmailAsync(mobno.Email);
                    if (user != null)
                    {
                        var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                        if (ismailConfirmed == true)
                        {
                            await _UserManager.RemovePasswordAsync(user);
                            await _UserManager.AddPasswordAsync(user, "Password@123");
                            user.UpdatedDate = DateTime.Now;
                            var result = await _UserManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                Email Email = new Email(_context);
                                string s = Email.sendmail(mobno.MI_ID, mobno.Email, "StaffUserCreation", user.Id);
                                if (s.Equals("success"))
                                {
                                    mobno.message = "Please Check Your Email,User Name and Password is sent";
                                }
                            }
                        }
                    }
                    else
                    {
                        mobno.message = "Entered Email address is Not Exists!!";
                    }
                }
                return mobno;
            }
            else if (mobno.clickedlinkname.Equals("forgotpwd"))
            {
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
                    Email Email = new Email(_context);
                    string[] ar = new string[] { };
                    string s = Email.sendmailsignin(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp), ar);
                    mobno.message = "Please Check Your Email,OTP Is Sent To Your Email Id";
                }
            }
            return mobno;

        }

        [Route("twofactorloginOtp")]
        public async Task<LoginDTO> twofactorloginOtp([FromBody] LoginDTO mobno)
        {
            string emailid = "";
            long miid = 0;
            ApplicationUser user = new ApplicationUser();
            user = await _UserManager.FindByNameAsync(mobno.UserName);
            if (user != null)
            {
                var usermiid = _context.UserRoleWithInstituteDMO.Where(a => a.Id == user.Id).ToList();
                if (usermiid != null && usermiid.Count > 0)
                {
                    miid = usermiid.FirstOrDefault().MI_Id;
                }
                else
                {
                    mobno.message = "false";
                    return mobno;
                }

                var checkemp = _context.Staff_User_Login.Where(a => a.Id == user.Id).ToList();
                if (checkemp != null && checkemp.Count>0)
                {
                    emailid = _context.Multiple_Email_DMO.Where(t => t.HRME_Id == checkemp.FirstOrDefault().Emp_Code && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                }
                else
                {
                    emailid = user.NormalizedEmail;
                }

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
                    Email Email = new Email(_context);

                    string[] subs = mobno.logindetails.Split(',');
                    
                    string s = Email.sendmailsignin(17, emailid, "EMAILOTP2F", Convert.ToInt64(genotp), subs);
                    mobno.message = "true";

                    if (checkemp != null && checkemp.Count > 0)
                    {
                        try
                        {
                            string subject = "Sign-In OTP!";
                            string titletext = "Your VMS Sign-in OTP Is " + genotp;
                            string sound = "default";
                            var deviceidsnew = ""; var devicenew = "";
                            var devicelist = (from a in _context.HR_Master_Employee_DMO
                                              where (a.HRME_ActiveFlag == true && a.HRME_Id == checkemp.FirstOrDefault().Emp_Code)
                                              select new ISM_ComplaintsTaskListDTO
                                              {
                                                  HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                              }).Distinct().ToList();
                            if (devicelist.Count > 0)
                            {
                                int k = 0;
                                foreach (var deviceid in devicelist)
                                {
                                    if (k == 0)
                                    {
                                        deviceidsnew = '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                        k = 1;
                                    }
                                    else
                                    {
                                        deviceidsnew = deviceidsnew + "," + '"' + deviceid.HRME_AppDownloadedDeviceId + '"';
                                    }
                                }
                                devicenew = "[" + deviceidsnew + "]";
                            }

                            //callnotificationasy(devicenew, titletext, sound, subject);
                            try
                            {
                                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                                string url = "";
                                url = "https://fcm.googleapis.com/fcm/send";
                                List<string> notificationparams = new List<string>();
                                string daata = "";
                                long notId = 1;

                                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                           "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + subject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + titletext + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                                notificationparams.Add(daata.ToString());
                                var mycontent = notificationparams[0];
                                string postdata = mycontent.ToString();
                                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                                connection.ContentType = "application/json";
                                connection.MediaType = "application/json";
                                connection.Accept = "application/json";

                                connection.Method = "post";
                                connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";
                                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                                {
                                    requestwriter.Write(postdata);
                                }
                                string responsedata = string.Empty;

                                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                                {
                                    responsedata = responsereader.ReadToEnd();
                                    JObject joresponse1 = JObject.Parse(responsedata);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            else
            {
                mobno.message = "false";
            }

            return mobno;
        }        

        [Route("ForgotEmailstudent")]
        public async Task<LoginDTO> ForgotEmailstudent([FromBody] LoginDTO mobno)
        {

            List<Adm_M_Student> allstudents = new List<Adm_M_Student>();
            allstudents = _context.Adm_M_Student.Where(t => t.AMST_Id == mobno.AMST_Id).ToList();


            if (allstudents.Count() > 0)
            {
                List<StudentAppUserLoginDMO> Studentapplogin = new List<StudentAppUserLoginDMO>();
                Studentapplogin = _context.StudentAppUserLoginDMO.Where(t => t.AMST_ID == mobno.AMST_Id).ToList();
                if (Studentapplogin.Count() > 0)
                {
                    ApplicationUser user = new ApplicationUser();
                    user = await _UserManager.FindByIdAsync(Studentapplogin.FirstOrDefault().STD_APP_ID.ToString());
                    if (user != null)
                    {
                        var ismailConfirmed = await _UserManager.IsEmailConfirmedAsync(user);
                        if (ismailConfirmed == true)
                        {
                            await _UserManager.RemovePasswordAsync(user);
                            await _UserManager.AddPasswordAsync(user, "Password@123");
                            user.UpdatedDate = DateTime.Now;
                            var result = await _UserManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                Email Email = new Email(_context);
                                string s = Email.sendmail(allstudents.FirstOrDefault().MI_Id, allstudents.FirstOrDefault().AMST_emailId, "StaffUserCreation", user.Id);
                                if (s.Equals("success"))
                                {
                                    mobno.message = "Please Check Your Email,User Name and Password is sent";
                                }
                            }
                        }
                    }
                    else
                    {
                        mobno.message = "Entered Email address is Not Exists!!";
                    }
                }
                else
                {
                    mobno.message = "Username is Not Exists for this student!!";
                }
            }
            else
            {
                mobno.message = "Username is Not Exists for this Email!!";
            }

            return mobno;

        }

        [Route("Userupdate")]
        public async Task<string> Userupdate([FromBody] LoginDTO mobno)
        {

            string genotp = "";

            if (mobno.clickedlinkname.Equals("M"))
            {
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

                    SMS sms = new SMS(_context);

                    string s = await sms.sendSms(mobno.MI_ID, Convert.ToInt64(mobno.Mobile), "EMAILOTP", Convert.ToInt64(genotp));


                    mobno.message = "Please Check,OTP Is Sent To Your Mobile Number";
                }
            }

            return mobno.message;

        }

        [Route("ForgotOTPForEmailval")]
        public async Task<string> ForgotOTPForEmailval([FromBody] LoginDTO mobno)
        {

            string genotp = "";

            //int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

            //CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

            //CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
            //mobno.MI_ID = cmnds.IVRM_MI_Id;

            mobno.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (mobno.clickedlinkname.Equals("M"))
            {
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

                    SMS sms = new SMS(_context);

                    string s = await sms.sendSms(mobno.MI_ID, Convert.ToInt64(mobno.Mobile), "EMAILOTP", Convert.ToInt64(genotp));


                    mobno.message = "Please Check,OTP Is Sent To Your Mobile Number";
                }
            }
            else if (mobno.clickedlinkname.Equals("E"))
            {
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
                    Email Email = new Email(_context);
                    string[] arr = new string[] { };
                    string s = Email.sendmailsignin(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp), arr);


                    mobno.message = "Please Check,OTP Is Sent To Your Email Id";
                }
            }



            return mobno.message;

        }

        [Route("VerifyEmailOtp")]
        public string VerifyEmailOtp([FromBody] LoginDTO mobno)
        {
            var EMAILOTP = HttpContext.Session.GetString("EMAILOTP");
            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFE"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 15)
            {
                mobno.message = "OTP Expired";
            }
            else
            {
                if (mobno.EMAILOTP != null)
                {
                    if (mobno.EMAILOTP == EMAILOTP)
                    {
                        mobno.message = "Success";
                    }
                    else
                    {
                        mobno.message = "Fail";
                    }
                }
                else
                {
                    mobno.message = "otp";
                }
            }
            HttpContext.Session.SetString("EmailOTPStatus", mobno.message);
            return mobno.message;
        }


        [Route("VerifyEmailOtpgen")]
        public string VerifyEmailOtpgen([FromBody] LoginDTO mobno)
        {
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
                    mobno.message = "Fail";
                }
            }


            HttpContext.Session.SetString("EmailOTPStatus", mobno.message);

            return mobno.message;
        }

        [Route("getOTP")]
        public async Task<string> getOTP([FromBody] LoginDTO mobno)
        {
            string genotp = "";

            mobno = pre.forgotPassword(mobno);

            if (mobno.message == "Success")
            {
                CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                genotp = generate.getOTP();

                if (genotp.Length < 6)
                {
                    genotp = generate.getOTP();
                }
                else
                {

                    HttpContext.Session.SetString("OTP", genotp);
                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://trans.kapsystem.com/api/web2sms.php?&workingkey=Af7660f900c9df85ac5dfae4b055d75bd&sender=HHSJCP&to=" + mobno.mobileNo + "&message=Your OTP is " + genotp) as HttpWebRequest;
                    //optional
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();
                    mobno.message = "OTP is sent to your Mobile Number";
                }
            }
            else
            {
                mobno.message = "Mobile Number does not match with the entered UserName";
            }

            return mobno.message;
        }

        [Route("getDataMyInstitute/{id:int}")]
        public LoginDTO getDataMyInstitute(int id)
        {
            HttpContext.Session.SetInt32("Session_MI_Id", Convert.ToInt32(id));

            LoginDTO dto = new LoginDTO();
            dto = pre.getInstituteDataByRoleTypeName(id);

            if (dto != null)
            {
                // Added on 24-Mar-2017
                HttpContext.Session.SetInt32("RoleId", Convert.ToInt32(dto.roleId));

                // HttpContext.Session.SetInt32("UserId", Convert.ToInt32(dto.userId));

                //  HttpContext.Session.SetInt32("Role", Convert.ToInt32(dto.role));

                HttpContext.Session.SetInt32("ASMAY_Id", Convert.ToInt32(dto.ASMAY_Id));
            }

            return dto;
        }

        [Route("getrolewisepage")]
        public LoginDTO getrolpagedata([FromBody] LoginDTO mobno)
        {
            //Removing Session value when user logs in.
            //Added on 05-03-2018
            HttpContext.Session.Remove("Session_Trip_IsOnline");
            HttpContext.Session.Remove("Session_Trip_MI_Id");
            HttpContext.Session.Remove("Session_Trip_ASMAY_Id");
            mobno.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            mobno.RoleName = HttpContext.Session.GetString("RoleNme");
            mobno = pre.getrolpagewisedata(mobno);
            return mobno;
        }

        [Route("ForgotOTPForEmailval_new")]
        public async Task<string> ForgotOTPForEmailval_new([FromBody] LoginDTO mobno)
        {
            try
            {
                SMS sms = new SMS(_context);
                long trnsno = 0;

                string genotp = "";

                mobno.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

                //int vr_id = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));

                //CommonDTO cmn = pre.setSessionDataForVirtualSchool(vr_id);

                //CommonDTO cmnds = pre.setSessionDataForVirtualSchool(vr_id);
                //mobno.MI_ID = cmnds.IVRM_MI_Id;

                mobno.MI_ID = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

                trnsno = sms.getsmsno(mobno.MI_ID);

                if (mobno.clickedlinkname.Equals("M"))
                {
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

                        string s = await sms.sendSmsnewTemplet_new(mobno.MI_ID, Convert.ToInt64(mobno.Mobile), "EMAILOTP", mobno.userId, trnsno, "", Convert.ToInt64(genotp));

                        mobno.message = "Please Check,OTP Is Sent To Your Mobile Number";
                    }
                }

                else if (mobno.clickedlinkname.Equals("E"))
                {
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

                        Email Email = new Email(_context);
                        string[] arr = new string[] { };
                        string s = Email.sendmailsignin(mobno.MI_ID, mobno.Email, "EMAILOTP", Convert.ToInt64(genotp), arr);

                        mobno.message = "Please Check,OTP Is Sent To Your Email Id";
                    }
                }
            }
            catch (Exception ex)
            {
                mobno.message = "Something Went Wrong Please Contact Administrator";
                Console.WriteLine(ex.Message);
            }

            return mobno.message;
        }

    }
}
