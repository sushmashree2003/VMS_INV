using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.HRMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;

namespace IVRMUX.Controllers.com.vapstech.VMS.HRMS
{
    [Produces("application/json")]
    [Route("api/VmsOverall_Report")]
    public class VmsOverall_ReportController : Controller
    {
        VmsOverall_Delegate _delegate = new VmsOverall_Delegate();

        [Route("getalldetails/{id:int}")]
        public VmsoverallDTO getalldetails(int id)
        {
            VmsoverallDTO data = new VmsoverallDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));           
            return _delegate.getalldetails(data);
        }
        [HttpPost]
        [Route("getReport")]
        public VmsoverallDTO getReport([FromBody] VmsoverallDTO data)
        {                       
            return _delegate.getReport(data);
        }
    }
}

