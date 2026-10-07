using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_Master_TaskGroupDelegate
    {
        CommonDelegate<ISM_Master_TaskGroupDTO, ISM_Master_TaskGroupDTO> comm = new CommonDelegate<ISM_Master_TaskGroupDTO, ISM_Master_TaskGroupDTO>();

        public ISM_Master_TaskGroupDTO loaddata(ISM_Master_TaskGroupDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskGroupFacade/loaddata");
        }
        public ISM_Master_TaskGroupDTO savedata(ISM_Master_TaskGroupDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskGroupFacade/savedata");
        }
        public ISM_Master_TaskGroupDTO Editdata(ISM_Master_TaskGroupDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskGroupFacade/Editdata");
        }       
        public ISM_Master_TaskGroupDTO clientDecative(ISM_Master_TaskGroupDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskGroupFacade/clientDecative");
        }
        public ISM_Master_TaskGroupDTO DeptDeactive(ISM_Master_TaskGroupDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskGroupFacade/DeptDeactive");
        }
        public ISM_Master_TaskGroupDTO viewuploaddept(ISM_Master_TaskGroupDTO data)
        {
            return comm.POSTIssueManager(data, "ISM_Master_TaskGroupFacade/viewuploaddept");
        }
    }
}
