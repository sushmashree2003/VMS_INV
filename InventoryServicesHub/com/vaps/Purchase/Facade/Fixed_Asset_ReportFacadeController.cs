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
    public class Fixed_Asset_ReportFacadeController : Controller
    {
        // GET: api/values
        Fixed_Asset_ReportInterface _Inv;
        public Fixed_Asset_ReportFacadeController(Fixed_Asset_ReportInterface Inv)
        {
            _Inv = Inv;
        }
        [Route("getloaddata")]
        public Task<INV_FiexAssetDTO> getloaddata([FromBody] INV_FiexAssetDTO data)
        {
            return _Inv.getloaddata(data);
        }
        [Route("onreport")]
        public Task<INV_FiexAssetDTO> onreport([FromBody] INV_FiexAssetDTO data)
        {
            return _Inv.onreport(data);
        }

        


    }
}
