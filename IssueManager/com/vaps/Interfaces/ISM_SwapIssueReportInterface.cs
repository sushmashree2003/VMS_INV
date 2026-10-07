using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_SwapIssueReportInterface
    {
        Task<ISM_PlannerReportsDTO> getdetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getdetails_weekly(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getdeviationReport(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getdeviationReport_weekly(ISM_PlannerReportsDTO data);

    }
}
