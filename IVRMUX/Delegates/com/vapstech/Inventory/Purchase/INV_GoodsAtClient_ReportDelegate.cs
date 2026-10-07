using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_GoodsAtClient_ReportDelegate
    {
        CommonDelegate<INV_GoodsAtClientDTO, INV_GoodsAtClientDTO> COMINV = new CommonDelegate<INV_GoodsAtClientDTO, INV_GoodsAtClientDTO>();
        public INV_GoodsAtClientDTO getloaddata(INV_GoodsAtClientDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_GoodsAtClient_ReportFacade/getloaddata/");
        }
        public INV_GoodsAtClientDTO onreport(INV_GoodsAtClientDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_GoodsAtClient_ReportFacade/onreport/");
        }

        
    }
}
