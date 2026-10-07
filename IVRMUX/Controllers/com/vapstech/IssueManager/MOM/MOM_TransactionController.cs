using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.MOM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager.MOM
{
    [Route("api/[controller]")]
    public class MOM_TransactionController : Controller
    {
        MOM_TransactionDelegate lcd = new MOM_TransactionDelegate();
        
        // GET: api/values
        [Route("getDetails/{id:int}")]
        public MOM_TransactionDTO getonlineLeave(int id)
        {
            MOM_TransactionDTO data = new MOM_TransactionDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.getDetails(data);
        }

        [Route("savedata")]
        public MOM_TransactionDTO savedata([FromBody] MOM_TransactionDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return lcd.savedata(data);
        }
    }
}