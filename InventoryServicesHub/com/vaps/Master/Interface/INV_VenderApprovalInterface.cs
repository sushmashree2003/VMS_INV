using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Interface
{
    public interface INV_VenderApprovalInterface
    {
        Task<INV_MasterVendorEntryDTO> getloaddataOne(INV_MasterVendorEntryDTO data);
        Task<INV_MasterVendorEntryDTO> getloaddataTwo(INV_MasterVendorEntryDTO data);
        Task<INV_MasterVendorEntryDTO> UpdateSP(INV_MasterVendorEntryDTO data);
      
    }
}

