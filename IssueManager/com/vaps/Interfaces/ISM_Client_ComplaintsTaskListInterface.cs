using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_Client_ComplaintsTaskListInterface
    {
        Task<ISM_ComplaintsTaskListDTO> getdetails(ISM_ComplaintsTaskListDTO data);
        Task<ISM_ComplaintsTaskListDTO> gettaskdetails(ISM_ComplaintsTaskListDTO data);
        Task<ISM_ComplaintsTaskListDTO> getsearchtask(ISM_ComplaintsTaskListDTO data);
    }
}
