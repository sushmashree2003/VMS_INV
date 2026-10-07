using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager
{
  
    [Route("api/[controller]")]
    public class ISM_DailyReport_CheckListController : Controller
    {
        ISM_DailyReport_CheckListDelegate del = new ISM_DailyReport_CheckListDelegate();
        [Route("loaddata/{id:int}")]
        public ISM_DailyReport_CheckListDTO loaddata(int id)
        {
            ISM_DailyReport_CheckListDTO dto = new ISM_DailyReport_CheckListDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(dto);
        }

        [Route("getdata")]
        public ISM_DailyReport_CheckListDTO getdata([FromBody] ISM_DailyReport_CheckListDTO dto)
        {
            
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getdata(dto);
        }
    }
}