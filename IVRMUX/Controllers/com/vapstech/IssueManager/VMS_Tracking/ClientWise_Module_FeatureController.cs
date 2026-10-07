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
    public class ClientWise_Module_FeatureController : Controller
    {
        ClientWise_Module_Feature_Delegate _del  = new ClientWise_Module_Feature_Delegate();

        [Route("getloaddata/{id:int}")]
        public ClientWise_Module_Feature_DTO getloaddata(int id)
        {
            ClientWise_Module_Feature_DTO dto = new ClientWise_Module_Feature_DTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getloaddata(dto);
        }

        [Route("getproject")]
        public ClientWise_Module_Feature_DTO getproject([FromBody] ClientWise_Module_Feature_DTO dto)
        {
          
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getproject(dto);
        }
         [Route("getmodule")]
        public ClientWise_Module_Feature_DTO getmodule([FromBody] ClientWise_Module_Feature_DTO dto)
        {
          
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getmodule(dto);
        }

        [Route("getreport")]
        public ClientWise_Module_Feature_DTO getreport([FromBody]ClientWise_Module_Feature_DTO dto)
        {
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _del.getreport(dto);
        }


    }
}