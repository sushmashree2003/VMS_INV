using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_TaskRegisterReportInterface
    {
        Task<ISM_TaskCreationDTO> getdetails(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> getemployeelist(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> gettaskregisterReport(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> Deptselectiondetails(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> Desgselectiondetails(ISM_TaskCreationDTO data);
        //=========== makerchecker consolidate report
        Task<ISM_TaskCreationDTO> getconsuldatedetails(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> getmakercheckerReport(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> getmakercheckerview(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> ISM_TaskRegister(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> ISM_TaskReport(ISM_TaskCreationDTO data);
        Task<ISM_TaskCreationDTO> getmakerRejectedReport(ISM_TaskCreationDTO data);

    }
}
