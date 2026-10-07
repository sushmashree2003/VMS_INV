using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.TADA.Interface;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.TADA;

namespace IssueManager.com.vaps.Controllers
{
    [Produces("application/json")]
    [Route("api/CheckBookEntryApprovalFacade")]
    public class CheckBookEntryApprovalFacadeController : Controller
    {
        public CheckBookEntryApprovalInterface _objinter;
        

        public CheckBookEntryApprovalFacadeController(CheckBookEntryApprovalInterface parameter)
        {
            _objinter = parameter;
        }
        [Route("getalldata")]
        public CheckBookEntryApprovalDTO getalldata([FromBody]CheckBookEntryApprovalDTO data)
        {
            return _objinter.getalldata(data);
        }
        [Route("TadaAllwence")]
        public CheckBookEntryApprovalDTO TadaAllwence([FromBody]CheckBookEntryApprovalDTO data)
        {
            return _objinter.TadaAllwence(data);
        }
        [Route("savedta")]
        public CheckBookEntryApprovalDTO savedta([FromBody]CheckBookEntryApprovalDTO data)
        {
            return _objinter.savedta(data);
        }
        //ApprovalData
        [Route("ApprovalData")]
        public CheckBookEntryApprovalDTO ApprovalData([FromBody]CheckBookEntryApprovalDTO data)
        {
            return _objinter.ApprovalData(data);
        }
        //edit
        [Route("edit")]
        public CheckBookEntryApprovalDTO edit([FromBody]CheckBookEntryApprovalDTO data)
        {
            return _objinter.edit(data);
        }
        //deactive
        [Route("deactive")]
        public CheckBookEntryApprovalDTO deactive([FromBody]CheckBookEntryApprovalDTO data)
        {
            return _objinter.deactive(data);
        }

        //Getdata
        [Route("Getdata")]
        public CheckApprovalDTO Getdata([FromBody]CheckApprovalDTO data)
        {
            return _objinter.Getdata(data);
        }
    }
}