using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_Outward_ReportDelegate
    {
        CommonDelegate<INV_Outward_ReportDTO, INV_Outward_ReportDTO> COMINV = new CommonDelegate<INV_Outward_ReportDTO, INV_Outward_ReportDTO>();
        public INV_Outward_ReportDTO getloaddata(INV_Outward_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Outward_ReportFacade/getloaddata/");
        }
        public INV_Outward_ReportDTO onreport(INV_Outward_ReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Outward_ReportFacade/onreport/");
        }

        
    }
}
