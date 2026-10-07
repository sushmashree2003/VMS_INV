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
    public class VMS_Receipt_VoucherFacade : Controller
    {
        public VMS_Receipt_VoucherInterface inter;
        public VMS_Receipt_VoucherFacade(VMS_Receipt_VoucherInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public VMS_Receipt_VoucherDTO loaddata([FromBody] VMS_Receipt_VoucherDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public VMS_Receipt_VoucherDTO savedata([FromBody] VMS_Receipt_VoucherDTO data)
        {
            return inter.savedata(data);
        }
        //onchange
        [Route("onchange")]
        public VMS_Receipt_VoucherDTO onchange([FromBody] VMS_Receipt_VoucherDTO data)
        {
            return inter.onchange(data);
        }

        //savedataSource
        [Route("savedataSource")]
        public CheckBookEntryDTO savedataSource([FromBody] CheckBookEntryDTO data)
        {
            return inter.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public CheckBookEntryDTO clientDecative([FromBody] CheckBookEntryDTO data)
        {
            return inter.clientDecative(data);
        }
        //RecieptReport
        [Route("RecieptReport")]
        public VMS_Receipt_VoucherDTO RecieptReport([FromBody] VMS_Receipt_VoucherDTO data)
        {
            return inter.RecieptReport(data);
        }
    }
}
