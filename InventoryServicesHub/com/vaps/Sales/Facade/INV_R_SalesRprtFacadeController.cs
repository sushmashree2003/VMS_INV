using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;
using InventoryServicesHub.com.vaps.Interface;
using InventoryServicesHub.com.vaps.Sales.Interface;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryServicesHub.com.vaps.Sales.Controllers
{
    [Route("api/[controller]")]
    public class INV_R_SalesRprtFacadeController : Controller
    {
        // GET: api/values
        INV_Sales_ReprtInterface _Inv;
        public INV_R_SalesRprtFacadeController(INV_Sales_ReprtInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_Sales_ReportDTO> getloaddata([FromBody] INV_Sales_ReportDTO data)
        {
            return _Inv.getloaddata(data);
        }
       

        [Route("onreport")]
        public Task<INV_Sales_ReportDTO> onreport([FromBody] INV_Sales_ReportDTO data)
        {
            return _Inv.onreport(data);
        }




    }
}
