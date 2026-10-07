using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ISM_RoleDeptMappingInterface
    {
        ISM_RoleDeptMappingDTO loaddata(ISM_RoleDeptMappingDTO data);
        ISM_RoleDeptMappingDTO savedata(ISM_RoleDeptMappingDTO data);
        ISM_RoleDeptMappingDTO deptDecative(ISM_RoleDeptMappingDTO data);
    }
}
