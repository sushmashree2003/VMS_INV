using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IVRMUX.Controllers.com.vapstech.VMS.TADA
{
    [Produces("application/json")]
    [Route("api/TADA_Reports")]
    public class TADA_ReportsController : Controller
    {
        TADA_ReportsDelegetes _delegate = new TADA_ReportsDelegetes();

        [Route("getalldata/{id:int}")]
        public TADA_ApplyReportsDTO getalldata(int id)
        {
            TADA_ApplyReportsDTO data = new TADA_ApplyReportsDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delegate.getalldata(data);
        }
        [Route("Deptselectiondetails")]
        public TADA_ApplyReportsDTO Deptselectiondetails([FromBody]TADA_ApplyReportsDTO data)
        {
            return _delegate.Deptselectiondetails(data);
        }
        //Desgselectiondetails
        [Route("Desgselectiondetails")]
        public TADA_ApplyReportsDTO Desgselectiondetails([FromBody]TADA_ApplyReportsDTO data)
        {
            return _delegate.Desgselectiondetails(data);
        }
        //GetReport
        [Route("GetReport")]
        public TADA_ApplyReportsDTO GetReport([FromBody]TADA_ApplyReportsDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _delegate.GetReport(data);
        }
        [Route("Viewdetails")]
        public TADA_ApplyReportsDTO Viewdetails([FromBody]TADA_ApplyReportsDTO data)
        {
            return _delegate.Viewdetails(data);
        }

    }
}