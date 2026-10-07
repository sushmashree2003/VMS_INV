using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_Purchase_ReportInterface
    {
       Task<INV_PurchaseReportDTO> getloaddata(INV_PurchaseReportDTO data);
        Task<INV_PurchaseReportDTO> onreport(INV_PurchaseReportDTO data);


    }


}
