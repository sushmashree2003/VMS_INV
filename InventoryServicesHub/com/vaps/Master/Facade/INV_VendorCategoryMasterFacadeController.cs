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
    public class INV_VendorCategoryMasterFacadeController : Controller
    {
        public INV_VendorCategoryMasterInterface _VendorsCategory;

        public INV_VendorCategoryMasterFacadeController(INV_VendorCategoryMasterInterface VendorCategory)
        {
            _VendorsCategory = VendorCategory;
        }


        [Route("SaveData")]
        public async Task<INV_VendorCategoryMasterDTO> SaveData([FromBody]INV_VendorCategoryMasterDTO data)
        {
            return await _VendorsCategory.SaveData(data);
        }

        [Route("Toggle_button")]
        public async Task<INV_VendorCategoryMasterDTO> Toggle_button([FromBody]INV_VendorCategoryMasterDTO data)
        {
            return await _VendorsCategory.Toggle_button(data);
        }

        [Route("GridData")]
        public async Task<INV_VendorCategoryMasterDTO> GridData([FromBody]INV_VendorCategoryMasterDTO data)
        {
            return await _VendorsCategory.GridData(data);
        }
    }
}