using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class ISM_Master_SciencePark_VendorCostDelegate
    {
        CommonVMSDelegate<ISM_Master_SciencePark_VendorCostDto, ISM_Master_SciencePark_VendorCostDto> CMMICP = new CommonVMSDelegate<ISM_Master_SciencePark_VendorCostDto, ISM_Master_SciencePark_VendorCostDto>();

        public ISM_Master_SciencePark_VendorCostDto OnLoadData(ISM_Master_SciencePark_VendorCostDto dto)
        {
            return CMMICP.POSTData(dto, "ISM_Master_SciencePark_VendorCostFacade/OnLoadData/");
        }
        public ISM_Master_SciencePark_VendorCostDto Save(ISM_Master_SciencePark_VendorCostDto dto)
        {
            return CMMICP.POSTData(dto, "ISM_Master_SciencePark_VendorCostFacade/Save/");
        }
        public ISM_Master_SciencePark_VendorCostDto Deactivate(ISM_Master_SciencePark_VendorCostDto dto)
        {
            return CMMICP.POSTData(dto, "ISM_Master_SciencePark_VendorCostFacade/Deactivate/");
        }
    }
}
