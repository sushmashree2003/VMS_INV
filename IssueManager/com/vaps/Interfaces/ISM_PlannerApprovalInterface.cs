using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_PlannerApprovalInterface
    {
        Task<ISM_PlannerCreationDTO> getdetails(ISM_PlannerCreationDTO data);
        Task<ISM_PlannerCreationDTO> loaddata(ISM_PlannerCreationDTO data);
        Task<ISM_PlannerCreationDTO> getplannerlist(ISM_PlannerCreationDTO data);      
        Task<ISM_PlannerCreationDTO> getplannerdetails(ISM_PlannerCreationDTO data);
        ISM_PlannerCreationDTO updateplannerStatus(ISM_PlannerCreationDTO data);
        ISM_PlannerCreationDTO allupdateplannerStatus(ISM_PlannerCreationDTO data);
        ISM_PlannerCreationDTO saveAddTask(ISM_PlannerCreationDTO data);
        Task<ISM_PlannerCreationDTO> getextratask(ISM_PlannerCreationDTO data);
        Task<ISM_PlannerCreationDTO> getApprovedPlannerDetail(ISM_PlannerCreationDTO data);
        Task<ISM_PlannerCreationDTO> AutoPlannerApproval(ISM_PlannerCreationDTO data);
        Task<ISM_PlannerCreationDTO> deactiveemployee(ISM_PlannerCreationDTO data);
        Task<ISM_PlannerCreationDTO> saveemployee(ISM_PlannerCreationDTO data);


    }
}
