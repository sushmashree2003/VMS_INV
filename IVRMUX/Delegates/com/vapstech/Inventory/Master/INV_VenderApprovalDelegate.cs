using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Master
{
    public class INV_VenderApprovalDelegate
    {
        CommonDelegate<INV_MasterVendorEntryDTO, INV_MasterVendorEntryDTO> COMINV = new CommonDelegate<INV_MasterVendorEntryDTO, INV_MasterVendorEntryDTO>();

        public async Task<INV_MasterVendorEntryDTO> getloaddataOne(INV_MasterVendorEntryDTO data)
        {
            return await Task.Run(() => COMINV.POSTDataInventory(data, "INV_VenderApprovalFacade/getloaddataOne"));
        }

        public async Task<INV_MasterVendorEntryDTO> getloaddataTwo(INV_MasterVendorEntryDTO data)
        {
            return await Task.Run(() => COMINV.POSTDataInventory(data, "INV_VenderApprovalFacade/getloaddataTwo"));
        }

        public async Task<INV_MasterVendorEntryDTO> UpdateSP(INV_MasterVendorEntryDTO data)
        {
            return await Task.Run(() => COMINV.POSTDataInventory(data, "INV_VenderApprovalFacade/UpdateSP"));
        }
    }
}
