using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.VMS.TADA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IVRMUX.Controllers.com.vapstech.VMS.TADA
{
    [Produces("application/json")]
    [Route("api/TADAApproval")]
    public class TADAApprovalController : Controller
    {
        TADAApprovalDelegate _delegate = new TADAApprovalDelegate();


        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADAApprovalDTO getalldata(int id)
        {
            TADAApprovalDTO dto = new TADAApprovalDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }
        [Route("getfinaldata/{id:int}")]
        public TADAApprovalDTO getfinaldata(int id)
        {
            TADAApprovalDTO dto = new TADAApprovalDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getfinaldata(dto);


        }
        //getalldata
        //getalldocument
        [HttpGet]
        [Route("getalldocument/{id:int}")]
        public TADAApprovalDTO getalldocument(int id)
        {
            TADAApprovalDTO dto = new TADAApprovalDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldocument(dto);


        }
        [HttpPost]
       
        [Route("savedta")]
        public TADAApprovalDTO savedta([FromBody]TADAApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(data);
        }
        //savefinaldta
        [Route("savefinaldta")]
        public TADAApprovalDTO savefinaldta([FromBody]TADAApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savefinaldta(data);
        }
        [Route("edit")]
        public TADAApprovalDTO edit([FromBody]TADAApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edit(data);
        }
        //editTada
        [Route("editTada")]
        public TADAApprovalDTO editTada([FromBody]TADAApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.editTada(data);
        }
        //edit
        [Route("deactive")]

        public TADAApprovalDTO deactive([FromBody]TADAApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.deactive(data);
        }
        //Getdata
        [Route("getfile")]

        public TADAApprovalDTO getfile([FromBody]TADAApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getfile(data);
        }
        //GetSavedDetails
        [Route("GetSavedDetails")]

        public TADAApprovalDTO GetSavedDetails([FromBody]TADAApprovalDTO data)
        {
           // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.GetSavedDetails(data);
        }
        
         [Route("getfileedit")]

        public TADAApprovalDTO getfileedit([FromBody]TADAApprovalDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getfileedit(data);
        }
        //Uploadfile 
        [Route("Uploadfile")]

        public TADAApprovalDTO Uploadfile([FromBody]TADAApprovalDTO data)
        {
            // data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.Uploadfile(data);
        }
    }
}