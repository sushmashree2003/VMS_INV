using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
    [Route("api/[controller]")]
    public class ISM_NonEarnedReportController : Controller
    {
        public ISM_NonEarnedReportDelegate _objdel = new ISM_NonEarnedReportDelegate();
               
          [Route("getalldata/{id:int}")]
        public ISM_NonEarnedReportDTO getalldata(int id)
        {
            ISM_NonEarnedReportDTO data = new ISM_NonEarnedReportDTO();

            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _objdel.getalldata(data);
        }
        [Route("Deptselectiondetails")]
        public ISM_NonEarnedReportDTO Deptselectiondetails([FromBody]ISM_NonEarnedReportDTO data)
        {
            return _objdel.Deptselectiondetails(data);
        }
        //Desgselectiondetails
        [Route("Desgselectiondetails")]
        public ISM_NonEarnedReportDTO Desgselectiondetails([FromBody]ISM_NonEarnedReportDTO data)
        {
            return _objdel.Desgselectiondetails(data);
        }

        [Route("getempblockReport")]
        public ISM_NonEarnedReportDTO getempblockReport([FromBody]ISM_NonEarnedReportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.getempblockReport(data);
        }

    }
}
