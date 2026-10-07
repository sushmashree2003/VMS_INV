using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Sales
{
    public class INV_DepreciationDelegate
    {
        CommonDelegate<INV_DepreciationDTO, INV_DepreciationDTO> COMINV = new CommonDelegate<INV_DepreciationDTO, INV_DepreciationDTO>();
        public INV_DepreciationDTO getloaddata(INV_DepreciationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_DepreciationFacade/getloaddata/");
        }
        public INV_DepreciationDTO savedetails(INV_DepreciationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_DepreciationFacade/savedetails/");
        }
        //storechanges
        public INV_DepreciationDTO storechanges(INV_DepreciationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_DepreciationFacade/storechanges/");
        }
        //Itemchanges
        public INV_DepreciationDTO Itemchanges(INV_DepreciationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_DepreciationFacade/Itemchanges/");
        }
        //deactive
        public INV_DepreciationDTO deactive(INV_DepreciationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_DepreciationFacade/deactive/");
        }
    }
}
