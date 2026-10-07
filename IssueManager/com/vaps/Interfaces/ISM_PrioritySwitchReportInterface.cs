using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_PrioritySwitchReportInterface
    {
        Task<ISM_PrioritySwitchReportDTO> getdetails(ISM_PrioritySwitchReportDTO data);
        Task<ISM_PrioritySwitchReportDTO> getdetails_weekly(ISM_PrioritySwitchReportDTO data);
        Task<ISM_PrioritySwitchReportDTO> getdeviationReport(ISM_PrioritySwitchReportDTO data);
        Task<ISM_PrioritySwitchReportDTO> getdeviationReport_weekly(ISM_PrioritySwitchReportDTO data);

      
    }
}
