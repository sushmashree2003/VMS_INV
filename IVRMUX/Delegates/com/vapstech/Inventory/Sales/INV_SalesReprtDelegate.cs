using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_SalesReprtDelegate
    {
        CommonDelegate<INV_Sales_ReportDTO, INV_Sales_ReportDTO> COMINV = new CommonDelegate<INV_Sales_ReportDTO, INV_Sales_ReportDTO>();
        public INV_Sales_ReportDTO getloaddata(INV_Sales_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesRprtFacade/getloaddata/");
        }
        public INV_Sales_ReportDTO onreport(INV_Sales_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesRprtFacade/onreport/");
        }

        
    }
}
