using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using IVRMUX.Delegates.com.vapstech.VisitorsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VisitorsManagement;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VisitorsManagement
{
    [Route("api/[controller]")]
    public class AddVisitorsController : Controller
    {
        AddVisitorsDelegate delobj = new AddVisitorsDelegate();
        public DomainModelMsSqlServerContext _context;
       
        public AddVisitorsController(DomainModelMsSqlServerContext _cont)
        {
            _context = _cont;           
        }

        private readonly AddVisitorsController _service;

       




        // GET: api/<controller>

        [Route("getDetails/{id:int}")]
        public AddVisitorsDTO getDetails(int id)
        {
            AddVisitorsDTO dto = new AddVisitorsDTO();
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.MI_Id = id;
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getDetails(dto);
        }
        [Route("Edit")]
        public AddVisitorsDTO Edit([FromBody] AddVisitorsDTO dto)
        {
            //AddVisitorsDTO dto = new AddVisitorsDTO();
            //dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.EditDetails(dto);
        }
        [HttpPost]
        [Route("saveData")]
        public AddVisitorsDTO saveData([FromBody]AddVisitorsDTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveData(data);
        }
        [Route("deactivate")]
        public AddVisitorsDTO deactivate([FromBody] AddVisitorsDTO d)
        {
            //d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.deactivate(d);
        }

        [Route("changeofradio")]
        public AddVisitorsDTO changeofradio([FromBody] AddVisitorsDTO d)
        {
            //d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.changeofradio(d);
        }

        [Route("GetVisitorDetails")]
        public AddVisitorsDTO GetVisitorDetails([FromBody] AddVisitorsDTO d)
        {
            //d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GetVisitorDetails(d);
        }

        [Route("Getmultivisitordetails")]
        public AddVisitorsDTO Getmultivisitordetails([FromBody] AddVisitorsDTO d)
        {
            //d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.Getmultivisitordetails(d);
        }


        [Route("UpdateStatus")]
        public AddVisitorsDTO UpdateStatus([FromBody] AddVisitorsDTO d)
        {
            //d.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            d.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.UpdateStatus(d);
        }

        [Route("getAssignDetails/{id:int}")]
        public AddVisitorsDTO getAssignDetails(int id)
        {
            AddVisitorsDTO dto = new AddVisitorsDTO();
            dto.MI_Id = id;
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getAssignDetails(dto);
        }

        [Route("getVisitorAssignDetails")]
        public AddVisitorsDTO getVisitorAssignDetails([FromBody] AddVisitorsDTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.getVisitorAssignDetails(dto);
        }

        [Route("saveAssignedData")]
        public AddVisitorsDTO saveAssignedData([FromBody] AddVisitorsDTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveAssignedData(dto);
        }

        [Route("SearchAppVisitors")]
        public AddVisitorsDTO SearchAppVisitors([FromBody] AddVisitorsDTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.SearchAppVisitors(dto);
        }

        [Route("searchfilter")]
        public AddVisitorsDTO searchfilter([FromBody]AddVisitorsDTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.searchfilter(dto);
        }

        [Route("visitornameschange")]
        public AddVisitorsDTO visitornameschange([FromBody]AddVisitorsDTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.visitornameschange(dto);
        }

        [Route("GetVisitorAssginDetails")]
        public AddVisitorsDTO GetVisitorAssginDetails([FromBody]AddVisitorsDTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GetVisitorAssginDetails(dto);
        }

        [Route("addtocart")]
        public AddVisitorsDTO addtocart([FromBody]AddVisitorsDTO dto)
        {
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.addtocart(dto);
        }

        [Route("getotp")]
        public AddVisitorsDTO getotp([FromBody]AddVisitorsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
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
               

                string s = Email.sendmail_checkbox(data.MI_Id, data.VMMV_VisitorEmailid, "EMAILOTP", Convert.ToInt64(genotp));

                data.message = "Please Check,OTP Is Sent To Your Email Id";
            }
            return data;
        }

        [Route("getnewotpverification")]
        public AddVisitorsDTO getnewotpverification([FromBody]AddVisitorsDTO data)
        {
            var EMAILOTP = HttpContext.Session.GetString("EMAILOTP");
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            DateTime dt1 = Convert.ToDateTime(HttpContext.Session.GetString("exptimeFE"));
            DateTime dt2 = DateTime.Now;
            double totalminutes = (dt2 - dt1).TotalMinutes;

            if (totalminutes > 30)
            {
                data.message = "OTP Expired";
            }
            else
            {

                if (data.genotp == EMAILOTP)
                {
                    data.message = "Success";

                }
                else
                {
                    data.message = "Wrong Enter OTP !";
                }
            }
          //  HttpContext.Session.SetString("EmailOTPStatus", data.message);


            return data;
        }

        //Tenent Visitor Managment



        //[HttpPost("generateQRCode")]
        [Route("generateQRCode")]
        public AddVisitorsDTO GenerateQRCode([FromBody] AddVisitorsDTO data)
        {
                     data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.GenerateQRCode(data);
        }


        [Route("QRCODECertificates")]
        //public AddVisitorsDTO QRCODECertificates([FromBody]AddVisitorsDTO dto)
        //{
        //    dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
        //    return delobj.QRCODECertificates(dto);
        //}


        public AddVisitorsDTO QRCODECertificates([FromBody] AddVisitorsDTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            AddVisitorsDTO resultDto = delobj.QRCODECertificates(dto);
            return resultDto;
        }





    }
}