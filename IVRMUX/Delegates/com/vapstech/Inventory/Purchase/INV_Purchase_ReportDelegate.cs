using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_Purchase_ReportDelegate
    {
        CommonDelegate<INV_PurchaseReportDTO, INV_PurchaseReportDTO> COMINV = new CommonDelegate<INV_PurchaseReportDTO, INV_PurchaseReportDTO>();
        public INV_PurchaseReportDTO getloaddata(INV_PurchaseReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Purchase_ReportFacade/getloaddata/");
        }
        public INV_PurchaseReportDTO onreport(INV_PurchaseReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Purchase_ReportFacade/onreport/");
        }

        
    }
}
