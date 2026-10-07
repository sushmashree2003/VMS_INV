using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_ImplTaskMappingInterface
    {
        Task<ISM_ImplTaskMappingDTO> getdetails(ISM_ImplTaskMappingDTO data);
       Task<ISM_ImplTaskMappingDTO> gettaskdetails(ISM_ImplTaskMappingDTO data);
    
        ISM_ImplTaskMappingDTO savePlanner(ISM_ImplTaskMappingDTO data);
        ISM_ImplTaskMappingDTO saveTransfer(ISM_ImplTaskMappingDTO data);
        ISM_ImplTaskMappingDTO saveResponse(ISM_ImplTaskMappingDTO data);
        Task<ISM_ImplTaskMappingDTO> getsearchtask(ISM_ImplTaskMappingDTO data);
        Task<ISM_ImplTaskMappingDTO> getcount(ISM_ImplTaskMappingDTO data);
        ISM_ImplTaskMappingDTO deactive(ISM_ImplTaskMappingDTO data);
        Task<ISM_ImplTaskMappingDTO> deptfilter(ISM_ImplTaskMappingDTO data);
        ISM_ImplTaskMappingDTO gettaskAttachment(ISM_ImplTaskMappingDTO data);

    }
}
