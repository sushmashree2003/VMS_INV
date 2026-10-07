using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Master
{
    public class INV_VendorSignOffDelegate
    {
        CommonDelegate<INV_VendorSignOffDTO, INV_VendorSignOffDTO> COMINV = new CommonDelegate<INV_VendorSignOffDTO, INV_VendorSignOffDTO>();

        public INV_VendorSignOffDTO getloaddata(INV_VendorSignOffDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorSignOffFacade/getloaddata");
        }

        public INV_VendorSignOffDTO savedetails(INV_VendorSignOffDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorSignOffFacade/savedetails");
        }

        public INV_VendorSignOffDTO deactive(INV_VendorSignOffDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorSignOffFacade/deactive");
        }
    }
}
