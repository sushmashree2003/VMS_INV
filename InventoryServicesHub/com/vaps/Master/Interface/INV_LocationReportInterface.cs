using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Interface
{
   public  interface INV_LocationReportInterface
    {
       
        Task<INV_LocationReportDTO> getloaddata(INV_LocationReportDTO data);
        Task<INV_LocationReportDTO> onreport(INV_LocationReportDTO data);
        //onchangeloaddata
        Task<INV_LocationReportDTO> onchangeloaddata(INV_LocationReportDTO data);
    }
}
