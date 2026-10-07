using IVRMUX.Delegates.com.vapstech.FrontOffice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Controllers.com.vapstech.FrontOffice
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterHolidayWorkingTypeController : Controller
    {
  
        MasterHolidayWorkingTypeDelegate del = new MasterHolidayWorkingTypeDelegate();

        [Route("getalldetails/{id:int}")]

        public MasterHolidayWorkingTypeDTO getalldetails(int id)
        {
            MasterHolidayWorkingTypeDTO data = new MasterHolidayWorkingTypeDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            return del.getdata(data);
        }

         [Route("savedetails")]
        public MasterHolidayWorkingTypeDTO savedetails([FromBody] MasterHolidayWorkingTypeDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.savedetails(data);
        }
    }
}
