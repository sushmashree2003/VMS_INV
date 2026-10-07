using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_ComplaintsTaskListInterface
    {
        Task<ISM_ComplaintsTaskListDTO> getdetails(ISM_ComplaintsTaskListDTO data);
       Task<ISM_ComplaintsTaskListDTO> gettaskdetails(ISM_ComplaintsTaskListDTO data);

        ISM_ComplaintsTaskListDTO savePlanner(ISM_ComplaintsTaskListDTO data);
        ISM_ComplaintsTaskListDTO saveTransfer(ISM_ComplaintsTaskListDTO data);
        ISM_ComplaintsTaskListDTO saveResponse(ISM_ComplaintsTaskListDTO data);
        ISM_ComplaintsTaskListDTO saveMultiResponse(ISM_ComplaintsTaskListDTO data);
        Task<ISM_ComplaintsTaskListDTO> getsearchtask(ISM_ComplaintsTaskListDTO data);
        Task<ISM_ComplaintsTaskListDTO> getsearchtaskdetails(ISM_ComplaintsTaskListDTO data);
        Task<ISM_ComplaintsTaskListDTO> getcount(ISM_ComplaintsTaskListDTO data);
        ISM_ComplaintsTaskListDTO deactive(ISM_ComplaintsTaskListDTO data);
        Task<ISM_ComplaintsTaskListDTO> deptfilter(ISM_ComplaintsTaskListDTO data);
        ISM_ComplaintsTaskListDTO gettaskAttachment(ISM_ComplaintsTaskListDTO data);
        ISM_ComplaintsTaskListDTO GetTaskSearchDetails(ISM_ComplaintsTaskListDTO data);
        //SaveApprovaldata
        ISM_ComplaintsTaskListDTO SaveApprovaldata(ISM_ComplaintsTaskListDTO data);
        #region DuplicateTaskClose
        Task<ISM_ComplaintsTaskListDTO> getDuplicateTasksList(ISM_ComplaintsTaskListDTO data);
        
        ISM_ComplaintsTaskListDTO saveDuplicateTask(ISM_ComplaintsTaskListDTO data);
        Task<ISM_ComplaintsTaskListDTO> getDuplicateApprovalTasksList(ISM_ComplaintsTaskListDTO data);
        ISM_ComplaintsTaskListDTO approveOrRejectDuplicateTask(ISM_ComplaintsTaskListDTO data);
        
        #endregion

    }
}
