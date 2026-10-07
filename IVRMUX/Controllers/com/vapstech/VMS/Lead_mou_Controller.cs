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
    public class Lead_mou_Controller : Controller
    {
        Lead_mou_Delegate LTM = new Lead_mou_Delegate();
        //GET: api/<controller>



        [Route("OnloadGetDetails/{id:int}")]
        public Lead_Target_MappingDTO OnloadGetDetails(int id)
        {

            Lead_Target_MappingDTO dto = new Lead_Target_MappingDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.User_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return LTM.onloadgetdetails(dto);

        
        }
        
     
        [Route("lead_product")]
        public Lead_Target_MappingDTO lead_product([FromBody]Lead_Target_MappingDTO dto)
        {
            //dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LTM.lead_product(dto);
        }

        [Route("savedata")]
        public Lead_Target_MappingDTO savedata([FromBody]Lead_Target_MappingDTO dto)
        {
            //dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LTM.savedata(dto);
        }
        

        [Route("MOUViewdata")]
        public Lead_Target_MappingDTO MOUViewdata([FromBody]Lead_Target_MappingDTO dto)
        {
            //dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LTM.MOUViewdata(dto);
        }

        [Route("sendmail")]
        public Lead_Target_MappingDTO sendmail([FromBody]Lead_Target_MappingDTO dto)
        {
            //dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return LTM.sendmail(dto);
        }


        

    }
}
