using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Master
{
    public class INV_MasterVendorEntryDelegate
    {
        CommonDelegate<INV_MasterVendorEntryDTO, INV_MasterVendorEntryDTO> COMINV = new CommonDelegate<INV_MasterVendorEntryDTO, INV_MasterVendorEntryDTO>();

        public INV_MasterVendorEntryDTO getloaddata(INV_MasterVendorEntryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterVendorEntryFacade/GridData");
        }

        public INV_MasterVendorEntryDTO savedetails(INV_MasterVendorEntryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterVendorEntryFacade/SaveData");
        }

        public INV_MasterVendorEntryDTO deactive(INV_MasterVendorEntryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterVendorEntryFacade/Toggle_button");
        }

        public INV_MasterVendorEntryDTO CategoryDropDown(INV_MasterVendorEntryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterVendorEntryFacade/CategoryDropDown");
        }
    }
}
