using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_ClosingStock_ReportInterface
    {
       Task<INV_ClosingStockDTO> getloaddata(INV_ClosingStockDTO data);
        Task<INV_ClosingStockDTO> onreport(INV_ClosingStockDTO data);


    }


}
