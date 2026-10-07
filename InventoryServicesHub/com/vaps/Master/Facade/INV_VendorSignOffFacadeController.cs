using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryServicesHub.com.vaps.Master.Interface;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Inventory;

namespace InventoryServicesHub.com.vaps.Master.Facade
{
    [Route("api/[controller]")]
    public class INV_VendorSignOffFacadeController : Controller
    {
        public INV_VendorSignOffInterface _interface;

        public INV_VendorSignOffFacadeController(INV_VendorSignOffInterface inter)
        {
            _interface = inter;
        }

        [HttpPost]
        [Route("getloaddata")]
        public async Task<INV_VendorSignOffDTO> getloaddata([FromBody] INV_VendorSignOffDTO dto)
        {
            return await _interface.getloaddata(dto);
        }

        [HttpPost]
        [Route("savedetails")]
        public async Task<INV_VendorSignOffDTO> savedetails([FromBody] INV_VendorSignOffDTO dto)
        {
            return await _interface.savedetails(dto);
        }

        [HttpPost]
        [Route("deactive")]
        public async Task<INV_VendorSignOffDTO> deactive([FromBody] INV_VendorSignOffDTO dto)
        {
            return await _interface.deactive(dto);
        }
    }
}
