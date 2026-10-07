using PreadmissionDTOs.com.vaps.Inventory.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
  public  interface INV_Depreciation_ReportInterface
    {
        INV_Depreciation_ReportDTO getloaddata(INV_Depreciation_ReportDTO data);
        INV_Depreciation_ReportDTO onreport(INV_Depreciation_ReportDTO data);
        INV_Depreciation_ReportDTO DepreciationCalculation(INV_Depreciation_ReportDTO data);
        
    }
}
