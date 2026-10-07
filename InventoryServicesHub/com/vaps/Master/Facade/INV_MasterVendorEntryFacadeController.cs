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
    public class INV_MasterVendorEntryFacadeController : Controller
    {
        public INV_MasterVendorEntryInterface _VendorsEntry;

        public INV_MasterVendorEntryFacadeController(INV_MasterVendorEntryInterface VendorEntry)
        {
            _VendorsEntry = VendorEntry;
        }


        [Route("SaveData")]
        public async Task<INV_MasterVendorEntryDTO> SaveData([FromBody]INV_MasterVendorEntryDTO data)
        {
            return await _VendorsEntry.SaveData(data);
        }

        [Route("Toggle_button")]
        public async Task<INV_MasterVendorEntryDTO> Toggle_button([FromBody]INV_MasterVendorEntryDTO data)
        {
            return await _VendorsEntry.Toggle_button(data);
        }

        [HttpPost]
        [Route("GridData")]
        public async Task<INV_MasterVendorEntryDTO> GridData([FromBody]INV_MasterVendorEntryDTO data)
        {
            return await _VendorsEntry.GridData(data);
        }

        [HttpPost]
        [Route("CategoryDropDown")]
        public async Task<INV_MasterVendorEntryDTO> CategoryDropDown([FromBody]INV_MasterVendorEntryDTO data)
        {
            return await _VendorsEntry.CategoryDropDown(data);
        }

        //[Route("VendorPopUp")]
        //public async Task<INV_MasterVendorEntryDTO> VendorPopUp([FromBody]INV_MasterVendorEntryDTO data)
        //{
        //    return await _VendorsEntry.VendorPopUp(data);
        //}
    }
}