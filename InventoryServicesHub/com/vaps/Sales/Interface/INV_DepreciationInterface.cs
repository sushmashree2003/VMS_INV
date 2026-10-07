using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
   public interface INV_DepreciationInterface 
    {
        INV_DepreciationDTO getloaddata(INV_DepreciationDTO data);
        INV_DepreciationDTO savedetails(INV_DepreciationDTO data);
        //storechanges
        INV_DepreciationDTO storechanges(INV_DepreciationDTO data);
        //Itemchanges
        INV_DepreciationDTO Itemchanges(INV_DepreciationDTO data);
        //deactive
        INV_DepreciationDTO deactive(INV_DepreciationDTO data);
    }
}
