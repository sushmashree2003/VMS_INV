using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ISM_PlannerReportInterface
    {
        Task<ISM_PlannerReportsDTO> getdetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getplannerReport(ISM_PlannerReportsDTO data);
    }
}
