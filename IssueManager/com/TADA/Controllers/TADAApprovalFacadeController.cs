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
    [Route("api/TADAApprovalFacade")]
    public class TADAApprovalFacadeController : Controller
    {
        public TADAApprovalInterface _objinter;
        

        public TADAApprovalFacadeController(TADAApprovalInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public TADAApprovalDTO getalldata([FromBody]TADAApprovalDTO data)
        {
            return _objinter.getalldata(data);
        }
        //getfinaldata
        [Route("getfinaldata")]
        public TADAApprovalDTO getfinaldata([FromBody]TADAApprovalDTO data)
        {
            return _objinter.getfinaldata(data);
        }
        [Route("savedta")]
        public TADAApprovalDTO savedta([FromBody]TADAApprovalDTO data)
        {
            return _objinter.savedta(data);
        }
        //savefinaldta
        [Route("savefinaldta")]
        public TADAApprovalDTO savefinaldta([FromBody]TADAApprovalDTO data)
        {
            return _objinter.savefinaldta(data);
        }
        //edit
        [Route("edit")]
        public TADAApprovalDTO edit([FromBody]TADAApprovalDTO data)
        {
            return _objinter.edit(data);
        }
        //editTada
        [Route("editTada")]
        public TADAApprovalDTO editTada([FromBody]TADAApprovalDTO data)
        {
            return _objinter.editTada(data);
        }
        //deactive
        [Route("deactive")]
        public TADAApprovalDTO deactive([FromBody]TADAApprovalDTO data)
        {
            return _objinter.deactive(data);
        }

        //Getdata
        [Route("getfile")]
        public TADAApprovalDTO getfile([FromBody]TADAApprovalDTO data)
        {
            return _objinter.getfile(data);
        }
        //GetSavedDetails
        [Route("GetSavedDetails")]
        public TADAApprovalDTO GetSavedDetails([FromBody]TADAApprovalDTO data)
        {
            return _objinter.GetSavedDetails(data);
        }
        //getalldocument
        [Route("getalldocument")]
        public TADAApprovalDTO getalldocument([FromBody]TADAApprovalDTO data)
        {
            return _objinter.getalldocument(data);
        }
        //getfileedit
        [Route("getfileedit")]
        public TADAApprovalDTO getfileedit([FromBody]TADAApprovalDTO data)
        {
            return _objinter.getfileedit(data);
        }
        //Uploadfile
        [Route("Uploadfile")]
        public TADAApprovalDTO Uploadfile([FromBody]TADAApprovalDTO data)
        {
            return _objinter.Uploadfile(data);
        }
    }
}