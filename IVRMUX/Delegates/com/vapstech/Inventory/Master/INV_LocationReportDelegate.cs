using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory.Master
{
    public class INV_LocationReportDelegate
    {
        CommonDelegate<INV_LocationReportDTO, INV_LocationReportDTO> COMINV = new CommonDelegate<INV_LocationReportDTO, INV_LocationReportDTO>();
        public INV_LocationReportDTO getloaddata(INV_LocationReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_LocationReportFacade/getloaddata/");
        }
        public INV_LocationReportDTO onreport(INV_LocationReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_LocationReportFacade/onreport/");
        }
        //onchangeloaddata
        public INV_LocationReportDTO onchangeloaddata(INV_LocationReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_LocationReportFacade/onchangeloaddata/");
        }
    }
}
