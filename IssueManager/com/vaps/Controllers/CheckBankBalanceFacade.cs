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
    public class CheckBankBalanceFacade : Controller
    {
        public CheckBankBalanceInterface inter;
        public CheckBankBalanceFacade(CheckBankBalanceInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public CheckBankBalanceDTO loaddata([FromBody] CheckBankBalanceDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public CheckBankBalanceDTO savedata([FromBody] CheckBankBalanceDTO data)
        {
            return inter.savedata(data);
        }
        [Route("getReporttwo")]
        public CheckBankBalanceDTO getReporttwo([FromBody] CheckBankBalanceDTO data)
        {
            return inter.getReporttwo(data);
        }
        //ClearenceDate
        [Route("ClearenceDate")]
        public CheckBankBalanceDTO ClearenceDate([FromBody] CheckBankBalanceDTO data)
        {
            return inter.ClearenceDate(data);
        }
        [Route("EditData")]
        public CheckBankBalanceDTO Editdata([FromBody] CheckBankBalanceDTO data)
        {
            return inter.Editdata(data);
        }
        [Route("getinvoiceno")]
        public CheckBankBalanceDTO getinvoiceno([FromBody] CheckBankBalanceDTO data)
        {
            return inter.getinvoiceno(data);
        }

        [Route("clientDecative")]
        public CheckBankBalanceDTO clientDecative([FromBody] CheckBankBalanceDTO data)
        {
            return inter.clientDecative(data);
        }
        [Route("getProject")]
        public VMSMaster_ChequeBookdto getProject([FromBody] VMSMaster_ChequeBookdto data)
        {
            return inter.getProject(data);
        }
        [Route("getbom")]
        public CheckBankBalanceDTO getbom([FromBody] CheckBankBalanceDTO data)
        {
            return inter.getbom(data);
        }
        [Route("companychange")]
        public CheckBankBalanceDTO companychange([FromBody] CheckBankBalanceDTO data)
        {
            return inter.companychange(data);
        }
        [Route("viewdetails")]
        public VMSMaster_ChequeBookdto viewdetails([FromBody] VMSMaster_ChequeBookdto data)
        {
            return inter.viewdetails(data);
        }
        [Route("savepaymentdetailsrecord")]
        public VMSMaster_ChequeBookdto savepaymentdetailsrecord([FromBody] VMSMaster_ChequeBookdto data)
        {
            return inter.savepaymentdetailsrecord(data);
        }

        // Payment Configuration
        [Route("loaddataconfig")]
        public VMSMaster_ChequeBookdto loaddataconfig([FromBody] VMSMaster_ChequeBookdto data)
        {
            return inter.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public CheckBankBalanceDTO savedataconfig([FromBody] CheckBankBalanceDTO data)
        {
            return inter.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport")]
        public CheckBankBalanceDTO loaddatareport([FromBody] CheckBankBalanceDTO data)
        {
            return inter.loaddatareport(data);
        }

        [Route("getreport")]
        public CheckBankBalanceDTO getreport([FromBody] CheckBankBalanceDTO data)
        {
            return inter.getreport(data);
        }

        // Notification Details
        [Route("paymentnotification")]
        public CheckBankBalanceDTO paymentnotification([FromBody] CheckBankBalanceDTO data)
        {
            return inter.paymentnotification(data);
        }

        //[Route("getYear")]
        //public CheckBankBalanceDTO getYear([FromBody] CheckBankBalanceDTO data)
        //{
        //    return inter.getYear(data);
        //}
    }
}
