using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Legal;

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class LegalUpdateMasterFacadeController : Controller
    {
        public LegalUpdateMasterInterface _interface;

        public LegalUpdateMasterFacadeController(LegalUpdateMasterInterface _interf)
        {
            _interface = _interf;
        }

        [Route("savelegalcasedetails")]
        public Legal_CaseDetailsDTO savelegalcasedetails([FromBody] Legal_CaseDetailsDTO data)
        {
            return _interface.savelegalcasedetails(data);
        }
        [Route("SaveDocuments")]
        public Legal_CaseDetailsDTO SaveDocuments([FromBody] Legal_CaseDetailsDTO data)
        {
            return _interface.SaveDocuments(data);
        }

        [Route("savecasestatus")]
        public Legal_CaseDetailsDTO savecasestatus([FromBody] Legal_CaseDetailsDTO data)
        {
            return _interface.savecasestatus(data);
        }

        [Route("saveadvocate")]
        public Legal_CaseDetailsDTO saveadvocate([FromBody] Legal_CaseDetailsDTO data)
        {
            return _interface.saveadvocate(data);
        }

        [Route("saveacts")]
        public Legal_CaseDetailsDTO saveacts([FromBody] Legal_CaseDetailsDTO data)
        {
            return _interface.saveacts(data);
        }

        [Route("savecasehistory")]
        public Legal_CaseDetailsDTO savecasehistory([FromBody] Legal_CaseDetailsDTO data)
        {
            return _interface.savecasehistory(data);
        }

        [Route("savefinalorder")]
        public Legal_CaseDetailsDTO savefinalorder([FromBody] Legal_CaseDetailsDTO data)
        {
            return _interface.savefinalorder(data);
        }

        [Route("loaddata")]
        public Legal_CaseDetailsDTO loaddata([FromBody]Legal_CaseDetailsDTO data)
        {
            return _interface.loaddata(data);
        }

        [Route("ViewLegalCaseDetails")]
        public Legal_CaseDetailsDTO ViewLegalCaseDetails([FromBody]Legal_CaseDetailsDTO data)
        {
            return _interface.ViewLegalCaseDetails(data);
        }

        [Route("editdata")]
        public Legal_CaseDetailsDTO editdata([FromBody]Legal_CaseDetailsDTO data)
        {
            return _interface.editdata(data);
        }
    }
}