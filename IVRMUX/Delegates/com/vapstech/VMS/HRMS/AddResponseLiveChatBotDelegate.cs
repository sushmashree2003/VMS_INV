using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;


namespace IVRMUX.Delegates.com.vapstech.VMS.HRMS
{
    public class AddResponseLiveChatBotDelegate
    {


        CommonDelegate<AddResponseLiveChatBotDTO, AddResponseLiveChatBotDTO> COMMM = new CommonDelegate<AddResponseLiveChatBotDTO, AddResponseLiveChatBotDTO>();

        public AddResponseLiveChatBotDTO savedata(AddResponseLiveChatBotDTO data)
        {
            return COMMM.POSTVMS(data, "AddResponseLiveChatBotFacade/savedata");

        }
        public AddResponseLiveChatBotDTO saveansdata(AddResponseLiveChatBotDTO data)
        {
            return COMMM.POSTVMS(data, "AddResponseLiveChatBotFacade/saveansdata");

        }

        public AddResponseLiveChatBotDTO GetBotResponse(AddResponseLiveChatBotDTO data)
        {
            return COMMM.POSTVMS(data, "AddResponseLiveChatBotFacade/GetBotResponse");

        }

        public AddResponseLiveChatBotDTO GetAllFAQs(AddResponseLiveChatBotDTO data)
        {
            return COMMM.POSTVMS(data, "AddResponseLiveChatBotFacade/GetAllFAQs");

        }



    }
}
