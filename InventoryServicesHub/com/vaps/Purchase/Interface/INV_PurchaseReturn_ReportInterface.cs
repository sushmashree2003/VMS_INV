using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PurchaseReturn_ReportInterface
    {
       INV_PurchaseReturnDTO getloaddata(INV_PurchaseReturnDTO data);
        Task<INV_PurchaseReturnDTO> onreport(INV_PurchaseReturnDTO data);


    }


}
