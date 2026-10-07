using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class InOutBoundCallDetailDelegate
    {

        CommonDelegate<ISM_Inbound_Outbound_Call_DetailsDTO, ISM_Inbound_Outbound_Call_DetailsDTO> COMFRNT = new CommonDelegate<ISM_Inbound_Outbound_Call_DetailsDTO, ISM_Inbound_Outbound_Call_DetailsDTO>();

        public ISM_Inbound_Outbound_Call_DetailsDTO getdata(ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "InOutBoundCallDetailFacade/getalldetails/");
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO report(ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "InOutBoundCallDetailFacade/report/");
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO savedata(ISM_Inbound_Outbound_Call_DetailsDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "InOutBoundCallDetailFacade/savedata/");
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO getdetails(int id)
        {
            return COMFRNT.DeleteDataByIdFROFF(id, "InOutBoundCallDetailFacade/getdetails/");
        }
        public ISM_Inbound_Outbound_Call_DetailsDTO deletedata(int id)
        {
            return COMFRNT.DeleteDataByIdFROFF(id, "InOutBoundCallDetailFacade/deletedata/");
        }
    }
}
