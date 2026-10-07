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
    [Route("api/TADAAdvanceApproval")]
    public class TADAAdvanceApprovalController : Controller
    {
        TADAAdvanceApprovalDelegate _delegate = new TADAAdvanceApprovalDelegate();


        [HttpGet]
        [Route("getalldata/{id:int}")]
        public TADAAdvanceApprovalDTO getalldata(int id)
        {
            TADAAdvanceApprovalDTO dto = new TADAAdvanceApprovalDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getalldata(dto);


        }

        //getTourApproval
        [Route("getTourApproval/{id:int}")]
        public TADAAdvanceApprovalDTO getTourApproval(int id)
        {
            TADAAdvanceApprovalDTO dto = new TADAAdvanceApprovalDTO();
            dto.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getTourApproval(dto);


        }
        [HttpPost]
       
        [Route("savedta")]
        public TADAAdvanceApprovalDTO savedta([FromBody]TADAAdvanceApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.savedta(data);
        }
        [Route("edit")]
        public TADAAdvanceApprovalDTO edit([FromBody]TADAAdvanceApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.edit(data);
        }
        //editTour
        [Route("editTour")]
        public TADAAdvanceApprovalDTO editTour([FromBody]TADAAdvanceApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.editTour(data);
        }
        //getView
        [Route("getView")]
        public TADAAdvanceApprovalDTO getView([FromBody]TADAAdvanceApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.getView(data);
        }
        //TadaAllwence
        [Route("TadaAllwence")]
        public TADAAdvanceApprovalDTO TadaAllwence([FromBody]TADAAdvanceApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TadaAllwence(data);
        }
        //updateallowence
        [Route("updateallowence")]
        public TADAAdvanceApprovalDTO updateallowence([FromBody]TADAAdvanceApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.updateallowence(data);
        }
        //CalculateTada
        [Route("CalculateTada")]
        public TADAAdvanceApprovalDTO CalculateTada([FromBody]TADAAdvanceApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.CalculateTada(data);
        }
        //TourApproval
        [Route("TourApproval")]
        public TADAAdvanceApprovalDTO TourApproval([FromBody]TADAAdvanceApprovalDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delegate.TourApproval(data);
        }
    }
}