using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Interface
{
    public interface INV_VendorCategoryMasterInterface
    {
        Task<INV_VendorCategoryMasterDTO> SaveData(INV_VendorCategoryMasterDTO data);
        Task<INV_VendorCategoryMasterDTO> Toggle_button(INV_VendorCategoryMasterDTO data);
        Task<INV_VendorCategoryMasterDTO> GridData(INV_VendorCategoryMasterDTO data);
    }
}
