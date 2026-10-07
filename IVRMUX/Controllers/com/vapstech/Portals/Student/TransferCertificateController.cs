using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.Student;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.Student
{
    [Route("api/[controller]")]
    public class TransferCertificateController : Controller
    {
        // GET: api/<controller>

        TransferCertificateDelegate _delobj = new TransferCertificateDelegate();

        [Route("getdetails/{id:int}")]
        public TransferCertificate_DTO getdetails(int id)
        {
            TransferCertificate_DTO data = new TransferCertificate_DTO();

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.AMST_Id = Convert.ToInt64(HttpContext.Session.GetInt32("AMST_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt64(HttpContext.Session.GetInt32("RoleId"));

            return _delobj.getdetails(data);
        }

        [Route("tcApply")]
        public TransferCertificate_DTO tcApply([FromBody] TransferCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));

            return _delobj.tcApply(data);
        }


        [Route("deactiveY")]
        public TransferCertificate_DTO deactiveY([FromBody]TransferCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _delobj.deactiveY(data);
        }

        [Route("certificateApproved")]
        public TransferCertificate_DTO certificateApproved([FromBody]TransferCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delobj.certificateApproved(data);
        }

        [Route("certificateRejected")]
        public TransferCertificate_DTO certificateRejected([FromBody]TransferCertificate_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delobj.certificateRejected(data);
        }


    }
}
