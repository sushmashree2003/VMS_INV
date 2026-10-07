using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_SalesReturn_ReportDelegate
    {
        CommonDelegate<INV_SalesReturnDTO, INV_SalesReturnDTO> COMINV = new CommonDelegate<INV_SalesReturnDTO, INV_SalesReturnDTO>();
        public INV_SalesReturnDTO getloaddata(INV_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesReturnFacade/getloaddata/");
        }
        public INV_SalesReturnDTO onreport(INV_SalesReturnDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_SalesReturnFacade/onreport/");
        }

        
    }
}
