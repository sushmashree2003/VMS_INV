using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Interface
{
    public interface INV_MasterVendorEntryInterface
    {
        Task<INV_MasterVendorEntryDTO> SaveData(INV_MasterVendorEntryDTO data);
        Task<INV_MasterVendorEntryDTO> Toggle_button(INV_MasterVendorEntryDTO data);
        Task<INV_MasterVendorEntryDTO> GridData(INV_MasterVendorEntryDTO data);
        Task<INV_MasterVendorEntryDTO> CategoryDropDown(INV_MasterVendorEntryDTO data);
        //Task<INV_MasterVendorEntryDTO> VendorPopUp(INV_MasterVendorEntryDTO data);k
    }
}
