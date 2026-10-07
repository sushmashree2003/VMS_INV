using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class Fixed_Asset_ReportDelegate
    {
        
        CommonDelegate<INV_FiexAssetDTO, INV_FiexAssetDTO> COMINV = new CommonDelegate<INV_FiexAssetDTO, INV_FiexAssetDTO>();
        public INV_FiexAssetDTO getloaddata(INV_FiexAssetDTO data)
        {
            return COMINV.POSTDataInventory(data, "Fixed_Asset_ReportFacade/getloaddata/");
        }
        public INV_FiexAssetDTO onreport(INV_FiexAssetDTO data)
        {
            return COMINV.POSTDataInventory(data, "Fixed_Asset_ReportFacade/onreport/");
        }

        
    }
}
