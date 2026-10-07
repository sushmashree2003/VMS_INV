using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_ClosingAtClient_ReportDelegate
    {
        CommonDelegate<INV_ClosingStockDTO, INV_ClosingStockDTO> COMINV = new CommonDelegate<INV_ClosingStockDTO, INV_ClosingStockDTO>();
        public INV_ClosingStockDTO getloaddata(INV_ClosingStockDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ClosingStock_ReportFacade/getloaddata/");
        }
        public INV_ClosingStockDTO onreport(INV_ClosingStockDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ClosingStock_ReportFacade/onreport/");
        }

   
    }
}
