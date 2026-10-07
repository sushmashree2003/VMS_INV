using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using Recruitment.com.vaps.Interfaces;

using PreadmissionDTOs.com.vaps.HRMS;




// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Recruitment.com.vaps.Controllers
{
    [Route("api/[controller]")]


    public class AddResponseLiveChatBotFacadeController : Controller
    {

        public AddResponseLiveChatBotInterface _abs;

        public AddResponseLiveChatBotFacadeController(AddResponseLiveChatBotInterface objf)
        {
            _abs = objf;
        }


        [Route("savedata")]
        public AddResponseLiveChatBotDTO savedata([FromBody]AddResponseLiveChatBotDTO data)
        {
            return _abs.savedata(data);
        }

        public AddResponseLiveChatBotDTO saveansdata([FromBody]AddResponseLiveChatBotDTO data)
        {
            return _abs.saveansdata(data);
        }

        [HttpPost("GetBotResponse")]
        public AddResponseLiveChatBotDTO GetBotResponse([FromBody] AddResponseLiveChatBotDTO data)
        {
            return _abs.GetBotResponse(data);
        }

        [HttpPost("GetAllFAQs")]
        public AddResponseLiveChatBotDTO GetAllFAQs([FromBody] AddResponseLiveChatBotDTO data)
        {
            return _abs.GetAllFAQs(data);
        }

    }
}
