using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using PreadmissionDTOs.com.vaps.VMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class ISM_Sales_BOMDelegate
    {

        CommonVMSDelegate<ISM_Sales_BOMDTO, ISM_Sales_BOMDTO> COMMC = new CommonVMSDelegate<ISM_Sales_BOMDTO, ISM_Sales_BOMDTO>();


        public ISM_Sales_BOMDTO getloaddata(ISM_Sales_BOMDTO dto)
        {
            return COMMC.POSTData(dto, "ISM_Sales_BOMFacade/getloaddata/");
        }

        public ISM_Sales_BOMDTO savedetails(ISM_Sales_BOMDTO dto)
        {
            return COMMC.POSTData(dto, "ISM_Sales_BOMFacade/savedetails/");
        }

        public ISM_Sales_BOMDTO saveiteamsdetails(ISM_Sales_BOMDTO dto)
        {
            return COMMC.POSTData(dto, "ISM_Sales_BOMFacade/saveiteamsdetails/");
        }
        public ISM_Sales_BOMDTO deactive(ISM_Sales_BOMDTO dto)
        {
            return COMMC.POSTData(dto, "ISM_Sales_BOMFacade/deactive/");
        }

        public ISM_Sales_BOMDTO saveBOMdetails(ISM_Sales_BOMDTO dto)
        {
            return COMMC.POSTData(dto, "ISM_Sales_BOMFacade/saveBOMdetails/");
        }

        public ISM_Sales_BOMDTO viewservicedetails(ISM_Sales_BOMDTO dto)
        {
            return COMMC.POSTData(dto, "ISM_Sales_BOMFacade/viewservicedetails/");
        }
        public ISM_Sales_BOMDTO Sales_BOM_edit(ISM_Sales_BOMDTO dto)
        {
            return COMMC.POSTData(dto, "ISM_Sales_BOMFacade/Sales_BOM_edit/");
        }

        
    }
}
