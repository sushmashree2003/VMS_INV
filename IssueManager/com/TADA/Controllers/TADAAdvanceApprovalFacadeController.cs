using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IssueManager.com.TADA.Controllers
{
    [Produces("application/json")]
    [Route("api/TADAAdvanceApprovalFacade")]
    public class TADAAdvanceApprovalFacadeController : Controller
    {
        public TADAAdvanceApprovalInterface _objinter;
        

        public TADAAdvanceApprovalFacadeController(TADAAdvanceApprovalInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADAAdvanceApprovalDTO getalldata([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.getalldata(data);
        }
        //getTourApproval
        [Route("getTourApproval")]
        public TADAAdvanceApprovalDTO getTourApproval([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.getTourApproval(data);
        }
        [Route("savedta")]
        public TADAAdvanceApprovalDTO savedta([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.savedta(data);
        }
        //edit
        [Route("edit")]
        public TADAAdvanceApprovalDTO edit([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.edit(data);
        }
        //editTour
        [Route("editTour")]
        public TADAAdvanceApprovalDTO editTour([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.editTour(data);
        }
        //getView
        [Route("getView")]
        public TADAAdvanceApprovalDTO getView([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.getView(data);
        }
        //TadaAllwence
        [Route("TadaAllwence")]
        public TADAAdvanceApprovalDTO TadaAllwence([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.TadaAllwence(data);
        }
        //updateallowence
        [Route("updateallowence")]
        public TADAAdvanceApprovalDTO updateallowence([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.updateallowence(data);
        }
        //CalculateTada
        [Route("CalculateTada")]
        public TADAAdvanceApprovalDTO CalculateTada([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.CalculateTada(data);
        }
        //TourApproval
        [Route("TourApproval")]
        public TADAAdvanceApprovalDTO TourApproval([FromBody]TADAAdvanceApprovalDTO data)
        {
            return _objinter.TourApproval(data);
        }
    }
}