using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Purchase.Controllers
{
    [Route("api/[controller]")]
    public class INV_PurchaseReturn_ReptFacadeController : Controller
    {
        // GET: api/values
        INV_PurchaseReturn_ReportInterface _Inv;
        public INV_PurchaseReturn_ReptFacadeController(INV_PurchaseReturn_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_PurchaseReturnDTO getloaddata([FromBody] INV_PurchaseReturnDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_PurchaseReturnDTO> onreport([FromBody] INV_PurchaseReturnDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
