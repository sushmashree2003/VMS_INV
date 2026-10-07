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
    public class INV_VenderApprovalFacadeController : Controller
    {
        public INV_VenderApprovalInterface _VendorApproval;

        public INV_VenderApprovalFacadeController(INV_VenderApprovalInterface VendorApproval)
        {
            _VendorApproval = VendorApproval;
        }

        [HttpPost]
        [Route("getloaddataOne")]
        public async Task<INV_MasterVendorEntryDTO> getloaddataOne([FromBody] INV_MasterVendorEntryDTO data)
        {
            return await _VendorApproval.getloaddataOne(data);
        }



        [HttpPost]
        [Route("getloaddataTwo")]
        public async Task<INV_MasterVendorEntryDTO> getloaddataTwo([FromBody] INV_MasterVendorEntryDTO data)
        {
            return await _VendorApproval.getloaddataTwo(data);
        }

        [HttpPost]
        [Route("UpdateSP")]
        public async Task<INV_MasterVendorEntryDTO> UpdateSP([FromBody] INV_MasterVendorEntryDTO data)
        {
            return await _VendorApproval.UpdateSP(data);

        }
    }

}