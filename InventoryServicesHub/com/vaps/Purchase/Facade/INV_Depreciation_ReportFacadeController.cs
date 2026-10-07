using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Purchase.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory.Purchase;

namespace InventoryServicesHub.com.vaps.Purchase.Facade
{
    [Produces("application/json")]
    [Route("api/INV_Depreciation_ReportFacade")]
    public class INV_Depreciation_ReportFacadeController : Controller
    {
        INV_Depreciation_ReportInterface _Inv;
        public INV_Depreciation_ReportFacadeController(INV_Depreciation_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public INV_Depreciation_ReportDTO getloaddata([FromBody] INV_Depreciation_ReportDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [HttpPost]
        [Route("onreport")]
        public INV_Depreciation_ReportDTO onreport([FromBody] INV_Depreciation_ReportDTO data)
        {
            return _Inv.onreport(data);
        }
        //DepreciationCalculation
        [Route("DepreciationCalculation")]
        public INV_Depreciation_ReportDTO DepreciationCalculation([FromBody] INV_Depreciation_ReportDTO data)
        {
            return _Inv.DepreciationCalculation(data);
        }
    }
}