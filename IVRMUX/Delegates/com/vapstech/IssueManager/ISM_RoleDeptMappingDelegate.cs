using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_RoleDeptMappingDelegate
    {
        CommonDelegate<ISM_RoleDeptMappingDTO, ISM_RoleDeptMappingDTO> comm = new CommonDelegate<ISM_RoleDeptMappingDTO, ISM_RoleDeptMappingDTO>();

        public ISM_RoleDeptMappingDTO loaddata(ISM_RoleDeptMappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_RoleDeptMappingFacade/loaddata");
        }
        
        public ISM_RoleDeptMappingDTO savedata(ISM_RoleDeptMappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_RoleDeptMappingFacade/savedata");
        }
        public ISM_RoleDeptMappingDTO deptDecative(ISM_RoleDeptMappingDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_RoleDeptMappingFacade/deptDecative");
        }
    }
}
