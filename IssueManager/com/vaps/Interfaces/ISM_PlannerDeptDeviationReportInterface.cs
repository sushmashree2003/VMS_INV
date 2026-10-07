using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_PlannerDeptDeviationReportInterface 
    {
        Task<ISM_PlannerReportsDTO> getdetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getdetails_weekly(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getdeviationReport(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> Deptselectiondetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> DeptselectionHeaddetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> gettaskdetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> gettaskdetailsreport(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> Desgselectiondetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> empcommunication (ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> apiempcommunication (ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getdeviationReport_weekly(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO getdaily_deviation(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO getEmp_ratingReport(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO taskRejectReport(ISM_PlannerReportsDTO data);
        
    }
}
