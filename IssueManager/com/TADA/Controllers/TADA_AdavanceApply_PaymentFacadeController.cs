using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IssueManager.com.TADA.Controllers
{
    [Produces("application/json")]
    [Route("api/TADA_AdavanceApply_PaymentFacade")]
    public class TADA_AdavanceApply_PaymentFacadeController : Controller
    {
        public TADA_AdavanceApply_PaymentInterface _objinter;

        public TADA_AdavanceApply_PaymentFacadeController(TADA_AdavanceApply_PaymentInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADA_AdavanceApply_PaymentDTO getalldata([FromBody]TADA_AdavanceApply_PaymentDTO data)
        {
            return _objinter.getalldata(data);
        }
        //getOnload
        [Route("getOnload")]
        public TADA_AdavanceApply_PaymentDTO getOnload([FromBody]TADA_AdavanceApply_PaymentDTO data)
        {
            return _objinter.getOnload(data);
        }
        //savedta
        [Route("savedta")]
        public TADA_AdavanceApply_PaymentDTO savedta([FromBody]TADA_AdavanceApply_PaymentDTO data)
        {
            return _objinter.savedta(data);
        }
        //savePayment
        [Route("savePayment")]
        public TADA_AdavanceApply_PaymentDTO savePayment([FromBody]TADA_AdavanceApply_PaymentDTO data)
        {
            return _objinter.savePayment(data);
        }
        ////deactive
        [Route("deactive")]
        public TADA_AdavanceApply_PaymentDTO deactive([FromBody]TADA_AdavanceApply_PaymentDTO data)
        {
            return _objinter.deactive(data);
        }
        //Viewdetails
        [Route("Viewdetails")]
        public TADA_AdavanceApply_PaymentDTO Viewdetails([FromBody]TADA_AdavanceApply_PaymentDTO data)
        {
            return _objinter.Viewdetails(data);
        }
        //edittt
        [Route("edittt")]
        public TADA_AdavanceApply_PaymentDTO edittt([FromBody]TADA_AdavanceApply_PaymentDTO data)
        {
            return _objinter.edittt(data);
        }
    }
}