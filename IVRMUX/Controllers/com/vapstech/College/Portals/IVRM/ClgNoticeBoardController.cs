using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;
using corewebapi18072016.Delegates.com.vapstech.College.Portals;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using corewebapi18072016.Delegates.com.vapstech.College.Portals.IVRM;
using PreadmissionDTOs.com.vaps.College.Portals.IVRM;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class ClgNoticeBoardController : Controller
    {
        ClgNoticeBoardDelegate clgdelegate = new ClgNoticeBoardDelegate();

        [HttpGet]
        [Route("getloaddata")]
        public ClgNoticeBoardDTO getloaddata(ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));                               
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getloaddata(data);
        }

        [Route("getbranchdata")]
        public ClgNoticeBoardDTO getbranchdata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getbranchdata(data);
        }

        [Route("getsemdata")]
        public ClgNoticeBoardDTO getsemdata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getsemdata(data);
        }
        [Route("savedata")]
        public ClgNoticeBoardDTO savedata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.savedata(data);
        }
        [Route("getNoticedata")]
        public ClgNoticeBoardDTO getNoticedata([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.getNoticedata(data);
        }

        [Route("editdetails")]
        public ClgNoticeBoardDTO editdetails([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.editdetails(data);
        }
        [Route("deactive")]
        public ClgNoticeBoardDTO deactive([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.deactive(data);
        }
        [Route("deactivedetails")]
        public ClgNoticeBoardDTO deactivedetails([FromBody]ClgNoticeBoardDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return clgdelegate.deactivedetails(data);
        }




    }
}
