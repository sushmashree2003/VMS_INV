using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface ISM_Sales_BOMInterface
    {
        ISM_Sales_BOMDTO getloaddata(ISM_Sales_BOMDTO dto);
        ISM_Sales_BOMDTO savedetails(ISM_Sales_BOMDTO dto);
        ISM_Sales_BOMDTO saveiteamsdetails(ISM_Sales_BOMDTO dto);
        ISM_Sales_BOMDTO deactive(ISM_Sales_BOMDTO dto);
        ISM_Sales_BOMDTO saveBOMdetails(ISM_Sales_BOMDTO dto);
        ISM_Sales_BOMDTO viewservicedetails(ISM_Sales_BOMDTO dto);
        ISM_Sales_BOMDTO Sales_BOM_edit(ISM_Sales_BOMDTO dto);
        

    }
}
