using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Legal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Legal;

namespace IVRMUX.Controllers.com.vapstech.Legal
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class LegalUpdateController : Controller
    {
        LegalUpdateDelegate delobj = new LegalUpdateDelegate();

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public LegalUpdateDTO loaddata(int id)
        {
            LegalUpdateDTO dto = new LegalUpdateDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.loaddata(dto);
        }

        [Route("saveData1")]
        public LegalUpdateDTO saveData1([FromBody]LegalUpdateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveData1(data);
        }
        [Route("validation")]
        public LegalUpdateDTO validation([FromBody] LegalUpdateDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.validation(data);
        }


    }
}