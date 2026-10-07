using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_Inward_ReportDelegate
    {
        CommonDelegate<INV_InwardDTO, INV_InwardDTO> COMINV = new CommonDelegate<INV_InwardDTO, INV_InwardDTO>();
        public INV_InwardDTO getloaddata(INV_InwardDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Inward_ReportFacade/getloaddata/");
        }
        public INV_InwardDTO onreport(INV_InwardDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Inward_ReportFacade/onreport/");
        }

        
    }
}
