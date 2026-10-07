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
    public class LeaveCreditController : Controller
    {
        LeaveCreditDelegate lcd = new LeaveCreditDelegate();

        // GET: api/values
        [HttpGet]
        [Route("GetLeaveCredit/{id:int}")]
        public LeaveCreditDTO GetLeaveCredit(int id)
        {
            LeaveCreditDTO lv = new LeaveCreditDTO();
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.GetLeaveCredit(lv);
        }

        [Route("get_departments")]
        public LeaveCreditDTO get_departments([FromBody] LeaveCreditDTO lv)
        { 
            lv.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_departments(lv);
        }

        [Route("get_designation")]
        public LeaveCreditDTO get_designation([FromBody] LeaveCreditDTO lvd)
        { 
            lvd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_designation(lvd);
        }

        [Route("get_grade")]
        public LeaveCreditDTO get_grade([FromBody] LeaveCreditDTO lvd)
        { 
            lvd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_grade(lvd);
        }

        [Route("get_leavecode")]
        public LeaveCreditDTO get_leavecode([FromBody] LeaveCreditDTO lc)
        {            
            lc.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return lcd.get_leavecode(lc);
        }

        [HttpPost]
        [Route("SaveData")]
        public LeaveCreditDTO SaveData([FromBody] LeaveCreditDTO lvcd)
        {
            lvcd.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            lvcd.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return lcd.SaveData(lvcd);
        }

        [Route("EditData")]
        public LeaveCreditDTO EditData([FromBody] LeaveCreditDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            return lcd.EditData(data);
        }

        //Manual Leave Credit
        [Route("OnLoadManualLeaveCredit")]
        public LeaveCreditDTO OnLoadManualLeaveCredit([FromBody] LeaveCreditDTO data)
        {
            if (data.OnLoadOrChangeInstFlag.Equals("OnLoad"))
            {
                data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            data.UserId= Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return lcd.OnLoadManualLeaveCredit(data);
        }
        
        [Route("SearchManualLeaveCreditDetails")]
        public LeaveCreditDTO SearchManualLeaveCreditDetails([FromBody] LeaveCreditDTO data)
        {            
            data.UserId= Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return lcd.SearchManualLeaveCreditDetails(data);
        }
        
        [Route("SaveManualLeaveCreditDetails")]
        public LeaveCreditDTO SaveManualLeaveCreditDetails([FromBody] LeaveCreditDTO data)
        {            
            data.UserId= Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return lcd.SaveManualLeaveCreditDetails(data);
        }
        [Route("viewmonthwiseleave")]
        public LeaveCreditDTO viewmonthwiseleave([FromBody] LeaveCreditDTO data)
        {
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return lcd.viewmonthwiseleave(data);
        }
    }
}
