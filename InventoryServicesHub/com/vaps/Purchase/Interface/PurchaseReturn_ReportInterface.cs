using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
   public interface PurchaseReturn_ReportInterface
    {

        PurchaseReturn_ReportDTO getdataReport(PurchaseReturn_ReportDTO data);
        PurchaseReturn_ReportDTO onreport(PurchaseReturn_ReportDTO data);
    }
}
