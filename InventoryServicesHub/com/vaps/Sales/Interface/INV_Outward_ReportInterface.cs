using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_Outward_ReportInterface
    {
       Task<INV_Outward_ReportDTO> getloaddata(INV_Outward_ReportDTO data);
        Task<INV_Outward_ReportDTO> onreport(INV_Outward_ReportDTO data);


    }


}
