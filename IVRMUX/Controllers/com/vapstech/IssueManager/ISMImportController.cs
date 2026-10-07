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
    public class ISMImportController : Controller
    {

        public ISMImportDelegate _objdel = new ISMImportDelegate();
        
        //[Route("Savedata")]
        public ISMImportDTO Savedata([FromBody]ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.Savedata(data);
        }

        [Route("saveIP")]
        public ISMImportDTO saveIP([FromBody]ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.saveIP(data);
        }
        [Route("saveadvance")]
        public ISMImportDTO saveadvance([FromBody]ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.saveadvance(data);
        }

        [Route("save_adv_planner_task")]
        public ISMImportDTO save_adv_planner_task([FromBody]ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.save_adv_planner_task(data);
        }



        //=====================================
        [Route("OnloadApproval")]
        public ISMImportDTO OnloadApproval([FromBody] ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.OnloadApproval(data);
        }

        [Route("getyearplannerreport")]
        public ISMImportDTO getyearplannerreport([FromBody] ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.getyearplannerreport(data);
        }

        [Route("yearplannerapp")]
        public ISMImportDTO yearplannerapp([FromBody]ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.yearplannerapp(data);
        }
        [Route("department")]
        public ISMImportDTO department([FromBody]ISMImportDTO data)
        {
           
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.department(data);
        }

        [Route("Depemplist")]
        public ISMImportDTO Depemplist([FromBody]ISMImportDTO data)
        {
           
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.Depemplist(data);
        }

        [Route("Depemptasklist")]
        public ISMImportDTO Depemptasklist([FromBody]ISMImportDTO data)
        {

            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _objdel.Depemptasklist(data);
        }


        [Route("swaptask_list")]
        public ISMImportDTO swaptask_list([FromBody]ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.swaptask_list(data);
        }
        [Route("swaptask_list_save")]
        public ISMImportDTO swaptask_list_save([FromBody]ISMImportDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.swaptask_list_save(data);
        }

    }
}
