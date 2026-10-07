using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PurchaseReturn_ReportDelegate
    {
        CommonDelegate<INV_PurchaseReturnDTO, INV_PurchaseReturnDTO> COMINV = new CommonDelegate<INV_PurchaseReturnDTO, INV_PurchaseReturnDTO>();
        public INV_PurchaseReturnDTO getloaddata(INV_PurchaseReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseReturn_ReptFacade/getloaddata/");
        }
        public INV_PurchaseReturnDTO onreport(INV_PurchaseReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseReturn_ReptFacade/onreport/");
        }

        
    }
}
