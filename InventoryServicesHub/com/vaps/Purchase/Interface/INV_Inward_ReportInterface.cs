using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_Inward_ReportInterface
    {
       Task<INV_InwardDTO> getloaddata(INV_InwardDTO data);
        Task<INV_InwardDTO> onreport(INV_InwardDTO data);


    }


}
