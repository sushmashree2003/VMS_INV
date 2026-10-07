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
    [Route("api/TADAApply_PaymentFACADE")]
    public class TADAApply_PaymentFACADEController : Controller
    {
        public TADAApply_PaymentInterFace _objinter;

        public TADAApply_PaymentFACADEController(TADAApply_PaymentInterFace parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADAApply_PaymentDTO getalldata([FromBody]TADAApply_PaymentDTO data)
        {
            return _objinter.getalldata(data);
        }
        //savedta
        [Route("savedta")]
        public TADAApply_PaymentDTO savedta([FromBody]TADAApply_PaymentDTO data)
        {
            return _objinter.savedta(data);
        }
        ////deactive
        [Route("deactive")]
        public TADAApply_PaymentDTO deactive([FromBody]TADAApply_PaymentDTO data)
        {
            return _objinter.deactive(data);
        }
        //edittt
        [Route("edittt")]
        public TADAApply_PaymentDTO edittt([FromBody]TADAApply_PaymentDTO data)
        {
            return _objinter.edittt(data);
        }
        [Route("Viewdetails")]
        public TADAApply_PaymentDTO Viewdetails([FromBody]TADAApply_PaymentDTO data)
        {
            return _objinter.Viewdetails(data);
        }
        //AutoSchedule
        [Route("AutoSchedule")]
        public TADAApply_PaymentDTO AutoSchedule([FromBody]TADAApply_PaymentDTO data)
        {
            return _objinter.AutoSchedule(data);
        }
      
    }
}