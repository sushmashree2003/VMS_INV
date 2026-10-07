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
    public class CheckBookEntryFacade : Controller
    {
        public CheckBookEntryInterface inter;
        public CheckBookEntryFacade(CheckBookEntryInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public CheckBookEntryDTO loaddata([FromBody] CheckBookEntryDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public CheckBookEntryDTO savedata([FromBody] CheckBookEntryDTO data)
        {
            return inter.savedata(data);
        }
        //savedataSource
        [Route("savedataSource")]
        public CheckBookEntryDTO savedataSource([FromBody] CheckBookEntryDTO data)
        {
            return inter.savedataSource(data);
        }
        [Route("EditData")]
        public CheckBookEntryDTO Editdata([FromBody] CheckBookEntryDTO data)
        {
            return inter.Editdata(data);
        }
        [Route("getinvoiceno")]
        public CheckBookEntryDTO getinvoiceno([FromBody] CheckBookEntryDTO data)
        {
            return inter.getinvoiceno(data);
        }

        [Route("clientDecative")]
        public CheckBookEntryDTO clientDecative([FromBody] CheckBookEntryDTO data)
        {
            return inter.clientDecative(data);
        }
        [Route("getProject")]
        public CheckBookEntryDTO getProject([FromBody] CheckBookEntryDTO data)
        {
            return inter.getProject(data);
        }
        [Route("getbom")]
        public CheckBookEntryDTO getbom([FromBody] CheckBookEntryDTO data)
        {
            return inter.getbom(data);
        }
        [Route("companychange")]
        public CheckBookEntryDTO companychange([FromBody] CheckBookEntryDTO data)
        {
            return inter.companychange(data);
        }
        [Route("viewdetails")]
        public CheckBookEntryDTO viewdetails([FromBody] CheckBookEntryDTO data)
        {
            return inter.viewdetails(data);
        }
        [Route("sms")]
        public CheckBookEntryDTO sms([FromBody] CheckBookEntryDTO data)
        {
            return inter.sms(data);
        }

        // Payment Configuration
        [Route("loaddataconfig")]
        public CheckBookEntryDTO loaddataconfig([FromBody] CheckBookEntryDTO data)
        {
            return inter.loaddataconfig(data);
        }

        [Route("savedataconfig")]
        public CheckBookEntryDTO savedataconfig([FromBody] CheckBookEntryDTO data)
        {
            return inter.savedataconfig(data);
        }

        // Payment subscription audit report
        [Route("loaddatareport")]
        public CheckBookEntryDTO loaddatareport([FromBody] CheckBookEntryDTO data)
        {
            return inter.loaddatareport(data);
        }

        [Route("getreport")]
        public CheckBookEntryDTO getreport([FromBody] CheckBookEntryDTO data)
        {
            return inter.getreport(data);
        }

        // Notification Details
        [Route("paymentnotification")]
        public CheckBookEntryDTO paymentnotification([FromBody] CheckBookEntryDTO data)
        {
            return inter.paymentnotification(data);
        }

        [Route("savedataEMI")]
        public CheckBookEntryDTO savedataEMI([FromBody] CheckBookEntryDTO data)
        {
            return inter.savedataEMI(data);
        }
       [HttpPost]
        [Route("viewuploadflies")]
        public CheckBookEntryDTO viewuploadflies([FromBody] CheckBookEntryDTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("geterifyEmail")]
        public CheckBookEntryDTO geterifyEmail([FromBody]CheckBookEntryDTO data)
        {
            return inter.geterifyEmail(data);
        }
    }
}
