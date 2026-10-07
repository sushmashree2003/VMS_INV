using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.IssueManager.VMS_Tracking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Controllers.com.vapstech.IssueManager.VMS_Tracking
{
 
    [Route("api/[controller]")]
    public class Fee_Payment_ManagementController : Controller
    {
        Fee_Payment_ManagementDelegate _del = new Fee_Payment_ManagementDelegate();

        [Route("getloaddata/{id:int}")]
        public Fee_Payment_ManagementDTO getloaddata (int id)
        {
            Fee_Payment_ManagementDTO dto = new Fee_Payment_ManagementDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getloaddata(dto);
        }

        [Route("getreport")]
        public Fee_Payment_ManagementDTO getreport(int id)
        {
            Fee_Payment_ManagementDTO dto = new Fee_Payment_ManagementDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getreport(dto);
        }
    }
}