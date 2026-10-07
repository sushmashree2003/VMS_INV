using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_Sales_ReprtInterface
    {
       Task<INV_Sales_ReportDTO> getloaddata(INV_Sales_ReportDTO data);
        Task<INV_Sales_ReportDTO> onreport(INV_Sales_ReportDTO data);


    }


}
