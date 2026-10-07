using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_Sales_ReportReturnInterface
    {
       Task<INV_SalesReturnDTO> getloaddata(INV_SalesReturnDTO data);
        Task<INV_SalesReturnDTO> onreport(INV_SalesReturnDTO data);


    }


}
