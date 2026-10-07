using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_Aging_ReportDelegate
    {
        CommonDelegate<INV_AgingDTO, INV_AgingDTO> COMINV = new CommonDelegate<INV_AgingDTO, INV_AgingDTO>();
        public INV_AgingDTO getloaddata(INV_AgingDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Aging_ReportFacade/getloaddata/");
        }
        public INV_AgingDTO onreport(INV_AgingDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Aging_ReportFacade/onreport/");
        }

        
    }
}
