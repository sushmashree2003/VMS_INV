using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_SwapIssuesInterface
    {
        ISM_SwapIssuesDTO loaddata(ISM_SwapIssuesDTO data);
        ISM_SwapIssuesDTO Report(ISM_SwapIssuesDTO data);
        ISM_SwapIssuesDTO Swaplist(ISM_SwapIssuesDTO data);
        ISM_SwapIssuesDTO Swaptask(ISM_SwapIssuesDTO data);
      
    }
}
