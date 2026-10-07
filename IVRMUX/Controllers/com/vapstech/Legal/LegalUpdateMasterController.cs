using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Legal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Legal;

namespace IVRMUX.Controllers.com.vapstech.Legal
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class LegalUpdateMasterController : Controller
    {
        LegalUpdateMasterDelegate del = new LegalUpdateMasterDelegate();
        //save

        [Route("savelegalcasedetails")]
        public Legal_CaseDetailsDTO savelegalcasedetails([FromBody] Legal_CaseDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savelegalcasedetails(data);
        }

        [Route("savecasestatus")]
        public Legal_CaseDetailsDTO savecasestatus([FromBody] Legal_CaseDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savecasestatus(data);
        }

        [Route("saveadvocate")]
        public Legal_CaseDetailsDTO saveadvocate([FromBody] Legal_CaseDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saveadvocate(data);
        }

        [Route("saveacts")]
        public Legal_CaseDetailsDTO saveacts([FromBody] Legal_CaseDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saveacts(data);
        }

        [Route("savecasehistory")]
        public Legal_CaseDetailsDTO savecasehistory([FromBody] Legal_CaseDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savecasehistory(data);
        }

        [Route("savefinalorder")]
        public Legal_CaseDetailsDTO savefinalorder([FromBody] Legal_CaseDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefinalorder(data);
        }

        [Route("SaveDocuments")]
        public Legal_CaseDetailsDTO SaveDocuments([FromBody] Legal_CaseDetailsDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.SaveDocuments(data);
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public Legal_CaseDetailsDTO loaddata(int id)
        {
            Legal_CaseDetailsDTO dto = new Legal_CaseDetailsDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(dto);
        }

        [Route("ViewLegalCaseDetails")]
        public Legal_CaseDetailsDTO ViewLegalCaseDetails([FromBody] Legal_CaseDetailsDTO data)
        {
            data.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.ViewLegalCaseDetails(data);
        }

        [Route("editdata")]
        public Legal_CaseDetailsDTO editdata([FromBody] Legal_CaseDetailsDTO data)
        {
            data.userId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return del.editdata(data);
        }
    }
}