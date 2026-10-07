using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Inventory.Purchase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace IVRMUX.Controllers.com.vapstech.Inventory.Purchase
{
   
    [Route("api/PurchaseReturn_Report")]
    public class PurchaseReturn_ReportController : Controller
    {
        PurchaseReturn_ReportDelegates _delegate = new PurchaseReturn_ReportDelegates();
       // [HttpGet]
        [Route("getdataReport/{id:int}")]
        public PurchaseReturn_ReportDTO getdataReport(int id)
        {
            PurchaseReturn_ReportDTO dto = new PurchaseReturn_ReportDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.getdataReport(dto);
        }
        [HttpPost]
        [Route("onreport")]
        public PurchaseReturn_ReportDTO onreport([FromBody] PurchaseReturn_ReportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return _delegate.onreport(data);
        }
    }
}