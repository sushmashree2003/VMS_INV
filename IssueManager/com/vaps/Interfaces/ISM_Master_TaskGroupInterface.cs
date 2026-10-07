using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ISM_Master_TaskGroupInterface
    {
        ISM_Master_TaskGroupDTO loaddata(ISM_Master_TaskGroupDTO data);
        ISM_Master_TaskGroupDTO savedata(ISM_Master_TaskGroupDTO data);
        ISM_Master_TaskGroupDTO Editdata(ISM_Master_TaskGroupDTO data);
        ISM_Master_TaskGroupDTO clientDecative(ISM_Master_TaskGroupDTO data);
        ISM_Master_TaskGroupDTO DeptDeactive(ISM_Master_TaskGroupDTO data);
        ISM_Master_TaskGroupDTO viewuploaddept(ISM_Master_TaskGroupDTO data);
    }
}
