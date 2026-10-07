using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using IVRMUX.Delegates.com.vapstech.VMS.HRMS;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.VMS.HRMS
{
    [Route("api/[controller]")]
    public class COETriggerController : Controller
    {
        COETriggerDelegate del = new COETriggerDelegate();

        // GET: api/values
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public COETriggerDTO getalldetails(int id)
        {
            COETriggerDTO dto = new COETriggerDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.onloadgetdetails(dto);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

      
        [Route("sendsmsemail")]
        public COETriggerDTO sendsmsemail([FromBody]COETriggerDTO dto)
        {

            if (dto.MI_Id == 0)
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.sendsmsemail(dto);
        }

        [Route("getcoetemplate")]
        public COETriggerDTO getcoetemplate([FromBody]COETriggerDTO dto)
        {

            
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            
            return del.getcoetemplate(dto);
        }

        [Route("editRecord/{id:int}")]
        public COETriggerDTO editRecord(int id)
        {
            COETriggerDTO dto = new COETriggerDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.HRMP_UpdatedBy = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getRecorddetailsById(id);
        }

        
        [Route("oncmpchange")]
        public COETriggerDTO oncmpchange([FromBody] COETriggerDTO dto)
        {

            return del.onloadgetdetails(dto);
        }
        [Route("getrpt")]
        public COETriggerDTO getrpt([FromBody] COETriggerDTO dto)
        {

            //   dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return del.getrpt(dto);
        }
       
        [Route("loadtemplate")]
        public COETriggerDTO loadtemplate([FromBody] COETriggerDTO dto)
        {
            
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
         

            return del.loadtemplate(dto);
        }

        [Route("viewtemplatedetails")]
        public COETriggerDTO viewtemplatedetails([FromBody] COETriggerDTO dto)
        {

            if (dto.MI_Id == 0)
            {
                dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            }
            return del.viewtemplatedetails(dto);
        }

    }
}
