using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.Fees;
using corewebapi18072016.Delegates.com.vapstech.Fees;

namespace corewebapi18072016.Controllers.com.vapstech.Fees
{
    [Route("api/[controller]")]
    [ValidateAntiForgeryToken]
         public class FeeMasterConcessionController : Controller
        {

            FeeMasterConcessionDelegate FGD = new FeeMasterConcessionDelegate();

            // GET: api/values
            [HttpGet]

            public IEnumerable<string> Get()
            {
                return new string[] { "value1", "value2" };
            }

        //[HttpPost]
        [Route("getdata/{id:int}")]
        public FeeMasterConcessionDTO getdata(int id)
        {
            FeeMasterConcessionDTO data = new FeeMasterConcessionDTO();
            data.MI_Id= Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.getdata(data);
        }

        [Route("savedata")]
        public FeeMasterConcessionDTO savedata([FromBody] FeeMasterConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.savedata(data);
        }

        [Route("activedeactive")]
        public FeeMasterConcessionDTO activedeactive([FromBody] FeeMasterConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.activedeactive(data);
        }

        [Route("editdata")]
        public FeeMasterConcessionDTO editdata([FromBody] FeeMasterConcessionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return FGD.editdata(data);
        }
    }

}
