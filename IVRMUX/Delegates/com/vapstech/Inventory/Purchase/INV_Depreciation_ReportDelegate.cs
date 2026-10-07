using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Purchase
{
    public class INV_Depreciation_ReportDelegate
    {
        CommonDelegate<INV_Depreciation_ReportDTO, INV_Depreciation_ReportDTO> COMINV = new CommonDelegate<INV_Depreciation_ReportDTO, INV_Depreciation_ReportDTO>();
        public INV_Depreciation_ReportDTO getloaddata(INV_Depreciation_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Depreciation_ReportFacade/getloaddata/");
        }
        public INV_Depreciation_ReportDTO onreport(INV_Depreciation_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Depreciation_ReportFacade/onreport/");
        }
        //DepreciationCalculation
        public INV_Depreciation_ReportDTO DepreciationCalculation(INV_Depreciation_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Depreciation_ReportFacade/DepreciationCalculation/");
        }
    }
}
