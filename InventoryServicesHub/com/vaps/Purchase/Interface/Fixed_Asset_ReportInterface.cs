using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface Fixed_Asset_ReportInterface
    {
       Task<INV_FiexAssetDTO> getloaddata(INV_FiexAssetDTO data);
        Task<INV_FiexAssetDTO> onreport(INV_FiexAssetDTO data);


    }


}
