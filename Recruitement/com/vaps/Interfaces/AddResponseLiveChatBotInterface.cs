using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.HRMS;


namespace Recruitment.com.vaps.Interfaces
{
    public interface AddResponseLiveChatBotInterface
    {
        AddResponseLiveChatBotDTO savedata(AddResponseLiveChatBotDTO data);
        AddResponseLiveChatBotDTO saveansdata(AddResponseLiveChatBotDTO data);




        AddResponseLiveChatBotDTO GetBotResponse(AddResponseLiveChatBotDTO data);
        AddResponseLiveChatBotDTO GetAllFAQs(AddResponseLiveChatBotDTO data);
    }
}
