using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Purchase
{
    public class PurchaseReturn_ReportDelegates
    {
        CommonDelegate<PurchaseReturn_ReportDTO, PurchaseReturn_ReportDTO> COMINV = new CommonDelegate<PurchaseReturn_ReportDTO, PurchaseReturn_ReportDTO>();
        
        public PurchaseReturn_ReportDTO getdataReport(PurchaseReturn_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "PurchaseReturn_ReportFacade/getdataReport/");



        }
        public PurchaseReturn_ReportDTO onreport(PurchaseReturn_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "PurchaseReturn_ReportFacade/onreport/");



        }
    }
}

