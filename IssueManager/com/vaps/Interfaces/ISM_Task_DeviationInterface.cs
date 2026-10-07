using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_Task_DeviationInterface
    {
        ISM_Task_DeviationDTO GetDetails(ISM_Task_DeviationDTO data);
        Task<ISM_Task_DeviationDTO> getdeviationReport(ISM_Task_DeviationDTO data);
        ISM_Task_DeviationDTO DeptSelectionDetails(ISM_Task_DeviationDTO data);
        ISM_Task_DeviationDTO DesgSelectionDetails(ISM_Task_DeviationDTO data);
        ISM_Task_DeviationDTO SaveDateExtention(ISM_Task_DeviationDTO data);

        ISM_Task_DeviationDTO OnloadApproval(ISM_Task_DeviationDTO data);

        ISM_Task_DeviationDTO Getrequestdetails(ISM_Task_DeviationDTO data);
        ISM_Task_DeviationDTO SaveApprovalExtention(ISM_Task_DeviationDTO data);
        ISM_Task_DeviationDTO ViewApproveDetails(ISM_Task_DeviationDTO data);

        

    }
}
