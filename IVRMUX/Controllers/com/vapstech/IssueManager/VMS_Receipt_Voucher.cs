using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class VMS_Receipt_Voucher : Controller
    {
        VMS_Receipt_VoucherDelegate del = new VMS_Receipt_VoucherDelegate();
        [Route("loaddata/{id:int}")]
        public VMS_Receipt_VoucherDTO loaddata(int id)
        {
            VMS_Receipt_VoucherDTO data = new VMS_Receipt_VoucherDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [HttpPost]
        [Route("savedata")]
        public VMS_Receipt_VoucherDTO savedata([FromBody]VMS_Receipt_VoucherDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.asmaY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return del.savedata(data);
        }
        [Route("onchange")]
        public VMS_Receipt_VoucherDTO onchange([FromBody]VMS_Receipt_VoucherDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.onchange(data);
        }
        //savedataSource
        [Route("savedataSource")]
        public CheckBookEntryDTO savedataSource([FromBody]CheckBookEntryDTO data)
        {


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.savedataSource(data);
        }
        //clientDecative
        [Route("clientDecative")]
        public CheckBookEntryDTO clientDecative([FromBody]CheckBookEntryDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.clientDecative(data);
        }
        //
        [Route("RecieptReport")]
        public VMS_Receipt_VoucherDTO RecieptReport([FromBody]VMS_Receipt_VoucherDTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return del.RecieptReport(data);
        }
    }
}
