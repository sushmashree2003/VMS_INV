using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface ISM_Master_SciencePark_VendorCostInterface
    {
        ISM_Master_SciencePark_VendorCostDto OnLoadData(ISM_Master_SciencePark_VendorCostDto dto);
        ISM_Master_SciencePark_VendorCostDto Save(ISM_Master_SciencePark_VendorCostDto dto);
        ISM_Master_SciencePark_VendorCostDto Deactivate (ISM_Master_SciencePark_VendorCostDto dto);
    }
}
