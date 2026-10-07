using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_SwapIssuesDelegate
    {
        CommonDelegate<ISM_SwapIssuesDTO, ISM_SwapIssuesDTO> _comm = new CommonDelegate<ISM_SwapIssuesDTO, ISM_SwapIssuesDTO>();

        public ISM_SwapIssuesDTO loaddata(ISM_SwapIssuesDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_SwapIssuesFacade/loaddata");
        }
     
        public ISM_SwapIssuesDTO Report(ISM_SwapIssuesDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_SwapIssuesFacade/Report");
        }
      public ISM_SwapIssuesDTO Swaplist(ISM_SwapIssuesDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_SwapIssuesFacade/Swaplist");
        }
        public ISM_SwapIssuesDTO Swaptask(ISM_SwapIssuesDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_SwapIssuesFacade/Swaptask");
        }
     
    }
}
