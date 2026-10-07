using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_PlannerListInterface
    {
        Task<ISM_PlannerCreationDTO> getdetails(ISM_PlannerCreationDTO data);
       Task<ISM_PlannerCreationDTO> gettaskdetails(ISM_PlannerCreationDTO data);
    
        ISM_PlannerCreationDTO savePlanner(ISM_PlannerCreationDTO data);
        ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data);
        ISM_PlannerCreationDTO removetask(ISM_PlannerCreationDTO data);     
    }
}
