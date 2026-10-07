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
    public class CheckBookPrintFacade : Controller
    {
        public CheckBookPrintInterface inter;
        public CheckBookPrintFacade(CheckBookPrintInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public CheckBookPrintDTO loaddata([FromBody] CheckBookPrintDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public CheckBookPrintDTO savedata([FromBody] CheckBookPrintDTO data)
        {
            return inter.savedata(data);
        }
        [Route("EditData")]
        public CheckBookPrintDTO Editdata([FromBody] CheckBookPrintDTO data)
        {
            return inter.Editdata(data);
        }
        [Route("getinvoiceno")]
        public CheckBookPrintDTO getinvoiceno([FromBody] CheckBookPrintDTO data)
        {
            return inter.getinvoiceno(data);
        }

        [Route("clientDecative")]
        public CheckBookPrintDTO clientDecative([FromBody] CheckBookPrintDTO data)
        {
            return inter.clientDecative(data);
        }
        [Route("getProject")]
        public CheckBookPrintDTO getProject([FromBody] CheckBookPrintDTO data)
        {
            return inter.getProject(data);
        }
        [Route("getbom")]
        public CheckBookPrintDTO getbom([FromBody] CheckBookPrintDTO data)
        {
            return inter.getbom(data);
        }
        [Route("companychange")]
        public CheckBookPrintDTO companychange([FromBody] CheckBookPrintDTO data)
        {
            return inter.companychange(data);
        }
        [Route("viewdetails")]
        public CheckBookPrintDTO viewdetails([FromBody] CheckBookPrintDTO data)
        {
            return inter.viewdetails(data);
        }
        [Route("savepaymentdetailsrecord")]
        public CheckBookPrintDTO savepaymentdetailsrecord([FromBody] CheckBookPrintDTO data)
        {
            return inter.savepaymentdetailsrecord(data);
        }

        // Payment Configuration
        [Route("loaddataconfig")]
        public CheckBookPrintDTO loaddataconfig([FromBody] CheckBookPrintDTO data)
        {
            return inter.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public CheckBookPrintDTO savedataconfig([FromBody] CheckBookPrintDTO data)
        {
            return inter.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport")]
        public CheckBookPrintDTO loaddatareport([FromBody] CheckBookPrintDTO data)
        {
            return inter.loaddatareport(data);
        }

        [Route("getreport")]
        public CheckBookPrintDTO getreport([FromBody] CheckBookPrintDTO data)
        {
            return inter.getreport(data);
        }

        // Notification Details
        [Route("paymentnotification")]
        public CheckBookPrintDTO paymentnotification([FromBody] CheckBookPrintDTO data)
        {
            return inter.paymentnotification(data);
        }

        //[Route("getYear")]
        //public CheckBookPrintDTO getYear([FromBody] CheckBookPrintDTO data)
        //{
        //    return inter.getYear(data);
        //}
    }
}
