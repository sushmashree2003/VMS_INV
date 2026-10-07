using corewebapi18072016.Delegates.com.vapstech.LeaveManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.LeaveManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Controllers.com.vapstech.LeaveManagement
{

    [Route("api/[controller]")]
    public class OnlineLeaveApplicationController : Controller
    {
        OnlineLeaveApplicationDelegate lcd = new OnlineLeaveApplicationDelegate();
        // GET: api/values
       
        
        [Route("getonlineLeave/{id:int}")]
        public LeaveCreditDTO getonlineLeave(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lv.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getonlineLeave(lv);
        }
       
        [Route("save")]
        public LeaveCreditDTO onlineleavesave([FromBody] LeaveCreditDTO test)
        {         
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id= Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.savedata(test);
        }

        [Route("saveadminLeave")]
        public LeaveCreditDTO saveadminLeave([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.saveadminLeave(test);
        }

        [Route("getonlineLeavestatus/{id:int}")]
        public LeaveCreditDTO getonlineLeavestatus(int id)
        {
            LeaveCreditDTO test = new LeaveCreditDTO();
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getonlineLeavestatus(test);
        }

        [Route("getSingleEmpLeavestatus/{empid:int}")]
        public LeaveCreditDTO getSingleEmpLeavestatus(int empid)
        {
            LeaveCreditDTO test = new LeaveCreditDTO();
            test.HRME_Id = empid;
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getSingleEmpLeavestatus(test);
        }

        [Route("getemployeeadmin/{id:int}")]
        public LeaveCreditDTO getemployeeadmin(int id)
        {
            LeaveCreditDTO test = new LeaveCreditDTO();
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getemployeeadmin(test);
        }
        
        [Route("requestleave")]
        public LeaveCreditDTO requestleave([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.requestleave(test);
        }

        [Route("viewcomment")]
        public LeaveCreditDTO viewcomment([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.viewcomment(test);
        }

        [Route("CheckAppliedDate")]
        public LeaveCreditDTO CheckAppliedDate([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.CheckAppliedDate(test);
        }

        [Route("CheckApprovalAuthorizationMapped")]
        public LeaveCreditDTO CheckApprovalAuthorizationMapped([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.CheckApprovalAuthorizationMapped(test);
        }

        [Route("Leaveapprover")]
        public LeaveCreditDTO Leaveapprover([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.Leaveapprover(test);
        }

        [Route("proxytaskdetails")]
        public LeaveCreditDTO proxytaskdetails([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.proxytaskdetails(test);
        }


        //----------------LRSaveUpdate-------------------------//

        [Route("LRdetails/{id:int}")]
        public LeaveCreditDTO LRdetails(int id)
        {
            LeaveCreditDTO lr = new LeaveCreditDTO();
            lr.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            lr.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.LRdetails(lr);
        }
        [Route("getdepartment")]
        public LeaveCreditDTO getdepartment([FromBody]LeaveCreditDTO data)
        {
            return lcd.getdepartment(data);
        }
        [Route("LRSaveUpdate")]
        public LeaveCreditDTO LRSaveUpdate([FromBody] LeaveCreditDTO test)
        {
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.LRSaveUpdate(test);
        }
        [Route("LREdit")]
        public LeaveCreditDTO LREdit([FromBody] LeaveCreditDTO test)
        {
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.LREdit(test);
        }
        [Route("ActiveDeactiveRecord")]
        public LeaveCreditDTO ActiveDeactiveRecord([FromBody] LeaveCreditDTO test)
        {
            //LeaveCreditDTO test = new LeaveCreditDTO();
            //test.HRLRD_Id = id;
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.ActiveDeactiveRecord(test);
        }
        [Route("leveConditions")]
        public LeaveCreditDTO leveConditions([FromBody] LeaveCreditDTO test)
        {
            test.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            test.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            test.asmay_id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return lcd.leveConditions(test);
        }
    }
}
