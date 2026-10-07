using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using corewebapi18072016.Delegates.com.vapstech.FrontOffice;
using corewebapi18072016.Delegates.com.vapstech.TT;
using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterDayController : Controller
    {
     
        FoMasterDayDelegate del = new FoMasterDayDelegate();
        [ValidateAntiForgeryToken]

        // GET: api/Academic/5
        [Route("getalldetails/{id:int}")]
        public MasterDayDTO getalldetails(int id)
        {
            MasterDayDTO data = new MasterDayDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return del.getalldetails(data);
        }

        [Route("savedetails")]
        public MasterDayDTO savedetails([FromBody] MasterDayDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(data);
        }

        [Route("deactive")]
         public MasterDayDTO deactive([FromBody] MasterDayDTO data)
        {
          data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
          data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
         return del.deactive(data);
        }

    }
}