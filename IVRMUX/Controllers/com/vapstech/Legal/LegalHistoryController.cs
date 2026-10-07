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
    public class LegalHistoryController : Controller
    {
        LegalHistoryDelegate delobj = new LegalHistoryDelegate();

        [Route("saveDataHistory")]
        public LegalHistoryDTO saveDataHistory([FromBody]LegalHistoryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return delobj.saveDataHistory(data);
        }
        [Route("checkvalidation1")]
        public LegalHistoryDTO checkvalidation1([FromBody] LegalHistoryDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return delobj.checkvalidation1(data);
        }
    }
}