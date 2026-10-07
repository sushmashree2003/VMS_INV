using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_Aging_ReportInterface
    {
       Task<INV_AgingDTO> getloaddata(INV_AgingDTO data);
        Task<INV_AgingDTO> onreport(INV_AgingDTO data);


    }


}
