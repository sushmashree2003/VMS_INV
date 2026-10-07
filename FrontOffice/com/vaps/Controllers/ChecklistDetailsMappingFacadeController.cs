using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontOffice.com.vaps.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.FrontOffice;

namespace FrontOffice.com.vaps.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
    public class ChecklistDetailsMappingFacadeController : Controller
    {
        public ISM_Checklist_DetailsMappingInterface _org;
        public ChecklistDetailsMappingFacadeController(ISM_Checklist_DetailsMappingInterface maspag)
        {
            _org = maspag;
        }

        [Route("getalldetails")]
        public ISM_Checklist_DetailsDTO getalldetails([FromBody] ISM_Checklist_DetailsDTO data)
        {
            return _org.getalldetails(data);
        }
        [Route("editRecord")]
        ///{id:int}
        public ISM_Checklist_DetailsDTO editRecord([FromBody] ISM_Checklist_DetailsDTO data)
        {
            return _org.editRecord(data);
        }
        [Route("DeleteYss")]
        ///{id:int}
        public ISM_Checklist_DetailsDTO DeleteYss([FromBody] ISM_Checklist_DetailsDTO data)
        {
            return _org.DeleteYss(data);
        }
        
        [Route("savedata")]
        public ISM_Checklist_DetailsDTO savedata([FromBody] ISM_Checklist_DetailsDTO data)
        {
            return _org.savedata(data);
        }

        [Route("approvalloaddata")]
        public ISM_Checklist_DetailsDTO approvalloaddata([FromBody] ISM_Checklist_DetailsDTO data)
        {
            return _org.approvalloaddata(data);
        }

        [Route("findchecklist")]
        public Task<ISM_Checklist_DetailsDTO> findchecklist([FromBody] ISM_Checklist_DetailsDTO data)
        {
            return _org.findchecklistAsync(data);
        }

        [Route("saveapproval")]
        public ISM_Checklist_DetailsDTO saveapproval([FromBody] ISM_Checklist_DetailsDTO data)
        {
            return _org.saveapproval(data);
        }

        [Route("checklistreport")]
        public ISM_Checklist_DetailsDTO checklistreport([FromBody] ISM_Checklist_DetailsDTO data)
        {
            return _org.checklistreport(data);
        }
        //[Route("employeereport")]
        //public ISM_Checklist_DetailsDTO employeereport([FromBody] ISM_Checklist_DetailsDTO data)
        //{
        //    return _org.employeereport(data);
        //}

    }
}