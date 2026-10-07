using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_Master_ClientProject_PaymentFacade : Controller
    {
        public ISM_Master_ClientProject_PaymentInterface inter;
        public ISM_Master_ClientProject_PaymentFacade(ISM_Master_ClientProject_PaymentInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public ISM_Master_ClientProject_PaymentDTO loaddata([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public ISM_Master_ClientProject_PaymentDTO savedata([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.savedata(data);
        }
        [Route("EditData")]
        public ISM_Master_ClientProject_PaymentDTO Editdata([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.Editdata(data);
        }

        [Route("clientDecative")]
        public ISM_Master_ClientProject_PaymentDTO clientDecative([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.clientDecative(data);
        }
        [Route("getProject")]
        public ISM_Master_ClientProject_PaymentDTO getProject([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.getProject(data);
        }
        [Route("viewdetails")]
        public ISM_Master_ClientProject_PaymentDTO viewdetails([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.viewdetails(data);
        }
        [Route("savepaymentdetailsrecord")]
        public ISM_Master_ClientProject_PaymentDTO savepaymentdetailsrecord([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.savepaymentdetailsrecord(data);
        }

        // Payment Configuration
        [Route("loaddataconfig")]
        public ISM_Master_ClientProject_PaymentDTO loaddataconfig([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public ISM_Master_ClientProject_PaymentDTO savedataconfig([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport")]
        public ISM_Master_ClientProject_PaymentDTO loaddatareport([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.loaddatareport(data);
        }

        [Route("getreport")]
        public ISM_Master_ClientProject_PaymentDTO getreport([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.getreport(data);
        }

        // Notification Details
        [Route("paymentnotification")]
        public ISM_Master_ClientProject_PaymentDTO paymentnotification([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        {
            return inter.paymentnotification(data);
        }

        //[Route("getYear")]
        //public ISM_Master_ClientProject_PaymentDTO getYear([FromBody] ISM_Master_ClientProject_PaymentDTO data)
        //{
        //    return inter.getYear(data);
        //}
    }
}
