using CommonLibrary;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.AssetTracking
{
    public class DCReportDelegate
    {
        CommonDelegate<DCReportDTO, DCReportDTO> COMAT = new CommonDelegate<DCReportDTO, DCReportDTO>();
        public DCReportDTO getloaddata(DCReportDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DCReportFacade/getloaddata/");
        }
      
        public DCReportDTO getreport(DCReportDTO data)
        {
            return COMAT.POSTDataAssetsTracking(data, "DCReportFacade/getreport/");
        }
       
        
    }
}
