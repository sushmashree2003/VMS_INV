using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Sales;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS
{
    [Route("api/[controller]")]
    public class Lead_Target_MappingnewController : Controller
    {
        Lead_Target_MappingnewDelegate LTM = new Lead_Target_MappingnewDelegate();
    


        [HttpGet]
        [Route("OnloadGetDetails/{id:int}")]
        public Lead_Target_MappingnewDTO OnloadGetDetails(int id)
        {
           
            Lead_Target_MappingnewDTO dto = new Lead_Target_MappingnewDTO();

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.onloadgetdetails(dto);
        }
 
       [Route("get_desig")]
        public Lead_Target_MappingnewDTO get_desig([FromBody] Lead_Target_MappingnewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.get_desig(dto);
        }

        
        [Route("get_employee")]
        public Lead_Target_MappingnewDTO get_employee([FromBody] Lead_Target_MappingnewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.get_employee(dto);
        }
        

        [Route("getdetails")]
        public Lead_Target_MappingnewDTO getdetails([FromBody] Lead_Target_MappingnewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.getdetails(dto);
        }


        [Route("getcatdetails")]
        public Lead_Target_MappingnewDTO getcatdetails([FromBody] Lead_Target_MappingnewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.getcatdetails(dto);
        }
        [Route("SaveTarget")]
        public Lead_Target_MappingnewDTO SaveTarget([FromBody] Lead_Target_MappingnewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.SaveTarget(dto);
        }

        [Route("Edit")]
        public Lead_Target_MappingnewDTO Edit([FromBody] Lead_Target_MappingnewDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.Edit(dto);
        }
    }
}
