using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ISM_CommonTaskInterface
    {
        ISM_CommonTask_DTO Loaddata(ISM_CommonTask_DTO data);
        ISM_CommonTask_DTO get_Designation(ISM_CommonTask_DTO data);
        ISM_CommonTask_DTO get_Employee(ISM_CommonTask_DTO data);
    }
}
