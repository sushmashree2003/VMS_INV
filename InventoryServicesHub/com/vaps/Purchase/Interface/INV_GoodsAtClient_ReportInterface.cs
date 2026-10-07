using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_GoodsAtClient_ReportInterface
    {
        Task<INV_GoodsAtClientDTO> getloaddata(INV_GoodsAtClientDTO data);
        Task<INV_GoodsAtClientDTO> onreport(INV_GoodsAtClientDTO data);


    }


}
