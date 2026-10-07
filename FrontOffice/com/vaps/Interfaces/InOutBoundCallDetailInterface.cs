using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
    public  interface InOutBoundCallDetailInterface
    {
        ISM_Inbound_Outbound_Call_DetailsDTO getdata(ISM_Inbound_Outbound_Call_DetailsDTO data);
        ISM_Inbound_Outbound_Call_DetailsDTO report(ISM_Inbound_Outbound_Call_DetailsDTO data);
        ISM_Inbound_Outbound_Call_DetailsDTO savedata(ISM_Inbound_Outbound_Call_DetailsDTO data);
        ISM_Inbound_Outbound_Call_DetailsDTO getdetails(int id);
        ISM_Inbound_Outbound_Call_DetailsDTO deletedata(int id);
    }
}
