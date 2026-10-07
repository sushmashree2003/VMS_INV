using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
namespace InventoryServicesHub.com.vaps.Purchase.Facade
{
    [Produces("application/json")]
   [Route("api/PurchaseReturn_ReportFacade")]
    public class PurchaseReturn_ReportFacade : Controller
    {
        PurchaseReturn_ReportInterface _Inv;

        public PurchaseReturn_ReportFacade(PurchaseReturn_ReportInterface Inv)
        {
            _Inv = Inv;
        }

        
        [Route("getdataReport")]
        public PurchaseReturn_ReportDTO getdataReport([FromBody] PurchaseReturn_ReportDTO data)
        {
            return _Inv.getdataReport(data);
        }
       
        [Route("onreport")]

        public PurchaseReturn_ReportDTO onreport([FromBody] PurchaseReturn_ReportDTO data)
        {
            return _Inv.onreport(data);
        }
    }
}