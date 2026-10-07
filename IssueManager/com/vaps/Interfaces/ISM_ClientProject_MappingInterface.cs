using IssueManager.com.vaps.Services;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_ClientProject_MappingInterface
    {
        ISM_ClientProject_MappingDTO loaddata(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO savedata(ISM_ClientProject_MappingDTO data);
        //added by praveenkumar 
        ISM_ClientProject_MappingDTO SaveApprovalVerify(ISM_ClientProject_MappingDTO data);
        //added by praveenkumar
        ISM_ClientProject_MappingDTO ViewMOUCheckUpFiles(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO SaveOrderAcceptanceApproval(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO Editdata(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO getproject(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO getmodule(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO clientDecative(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO savedataLead(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO viewtemplatedetails(ISM_ClientProject_MappingDTO data);
        ISM_ClientProject_MappingDTO OnChangeOnChangeBOMList(ISM_ClientProject_MappingDTO dto);
        Task<ISM_ClientProject_MappingDTO> sendMail(ISM_ClientProject_MappingDTO data);

        ISM_ClientProject_MappingDTO releaseversion(ISM_ClientProject_MappingDTO data);
    }
}
