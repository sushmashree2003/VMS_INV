using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface ISM_Checklist_DetailsMappingInterface
    {
        ISM_Checklist_DetailsDTO editRecord(ISM_Checklist_DetailsDTO id );
        ISM_Checklist_DetailsDTO DeleteYss(ISM_Checklist_DetailsDTO id);

        ISM_Checklist_DetailsDTO getalldetails(ISM_Checklist_DetailsDTO data);
        ISM_Checklist_DetailsDTO savedata(ISM_Checklist_DetailsDTO data);
        ISM_Checklist_DetailsDTO approvalloaddata(ISM_Checklist_DetailsDTO data);
        Task<ISM_Checklist_DetailsDTO> findchecklistAsync(ISM_Checklist_DetailsDTO data);
        ISM_Checklist_DetailsDTO saveapproval(ISM_Checklist_DetailsDTO data);
        ISM_Checklist_DetailsDTO checklistreport(ISM_Checklist_DetailsDTO data);
    }
}
