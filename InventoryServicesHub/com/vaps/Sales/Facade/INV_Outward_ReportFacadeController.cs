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
    public class INV_Outward_ReportFacadeController : Controller
    {
        // GET: api/values
        INV_Outward_ReportInterface _Inv;
        public INV_Outward_ReportFacadeController(INV_Outward_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_Outward_ReportDTO> getloaddata([FromBody] INV_Outward_ReportDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_Outward_ReportDTO> onreport([FromBody] INV_Outward_ReportDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
