using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueManager.com.vaps.Interfaces;
using IssueManager.com.vaps.Services;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.IssueManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IssueManager.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ISM_ClientProject_MappingFacade : Controller
    {
        public ISM_ClientProject_MappingInterface inter;
        public ISM_ClientProject_MappingFacade(ISM_ClientProject_MappingInterface s)
        {
            inter = s;
        }
        [Route("loaddata")]
        public ISM_ClientProject_MappingDTO loaddata([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.loaddata(data);
        }
        [Route("savedata")]
        public ISM_ClientProject_MappingDTO savedata([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.savedata(data);
        }
        //added by praveenkumar
        [Route("SaveApprovalVerify")]
        public ISM_ClientProject_MappingDTO SaveApprovalVerify([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.SaveApprovalVerify(data);
        }

        //added by praveenkumarSigned mou view details
        [Route("SaveOrderAcceptanceApproval")]
        public ISM_ClientProject_MappingDTO SaveOrderAcceptanceApproval([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.SaveOrderAcceptanceApproval(data);
        }
        [Route("ViewMOUCheckUpFiles")]
        public ISM_ClientProject_MappingDTO ViewMOUCheckUpFiles([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.ViewMOUCheckUpFiles(data);
        }
        [Route("EditData")]
        public ISM_ClientProject_MappingDTO Editdata([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.Editdata(data);
        }
        [Route("getproject")]
        public ISM_ClientProject_MappingDTO getproject([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.getproject(data);
        }
        [Route("getmodule")]
        public ISM_ClientProject_MappingDTO getmodule([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.getmodule(data);
        }
        [Route("clientDecative")]
        public ISM_ClientProject_MappingDTO clientDecative([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.clientDecative(data);
        }
        [Route("savedataLead")]
        public ISM_ClientProject_MappingDTO savedataLead([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.savedataLead(data);
        }
        [Route("viewtemplatedetails")]
        public ISM_ClientProject_MappingDTO viewtemplatedetails([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.viewtemplatedetails(data);
        }


        [Route("OnChangeOnChangeBOMList")]
        public ISM_ClientProject_MappingDTO OnChangeOnChangeBOMList([FromBody] ISM_ClientProject_MappingDTO dto)
        {
            return inter.OnChangeOnChangeBOMList(dto);
        }
        //sendMail

       
        [Route("sendMail")]
        public async Task<ISM_ClientProject_MappingDTO> sendMail([FromBody]ISM_ClientProject_MappingDTO data)
        {
            return await inter.sendMail(data);
        }
        [Route("releaseversion")]
        public ISM_ClientProject_MappingDTO releaseversion([FromBody] ISM_ClientProject_MappingDTO data)
        {
            return inter.releaseversion(data);
        }
    }
}
