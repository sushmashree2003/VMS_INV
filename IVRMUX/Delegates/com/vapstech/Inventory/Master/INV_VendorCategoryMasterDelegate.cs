using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_VendorCategoryMasterDelegate
    {
        CommonDelegate<INV_VendorCategoryMasterDTO, INV_VendorCategoryMasterDTO> COMINV = new CommonDelegate<INV_VendorCategoryMasterDTO, INV_VendorCategoryMasterDTO>();

        public INV_VendorCategoryMasterDTO getloaddata(INV_VendorCategoryMasterDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorCategoryMasterFacade/GridData");
        }

        public INV_VendorCategoryMasterDTO savedetails(INV_VendorCategoryMasterDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorCategoryMasterFacade/SaveData");
        }

        public INV_VendorCategoryMasterDTO deactive(INV_VendorCategoryMasterDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorCategoryMasterFacade/Toggle_button");
        }
    }
}
