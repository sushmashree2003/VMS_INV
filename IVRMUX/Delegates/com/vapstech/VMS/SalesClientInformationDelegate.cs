using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VMS
{
    public class SalesClientInformationDelegate
    {
        CommonVMSDelegate<SalesClientInformationDto, SalesClientInformationDto> CMMICP = new CommonVMSDelegate<SalesClientInformationDto, SalesClientInformationDto>();

        public SalesClientInformationDto OnLoadData(SalesClientInformationDto dto)
        {
            return CMMICP.POSTData(dto, "SalesClientInformationFacade/OnLoadData/");
        }
    }
}
