using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.IssueManager;
namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_ApprovalReportInterface
    {
        ISM_PlannerReportsDTO getdetails(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO getPlannerlist(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getApprovalReport(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getPlannerDReport(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> Desgselectiondetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> Deptselectiondetails(ISM_PlannerReportsDTO data);
        Task<ISM_PlannerReportsDTO> getempdetailsReport(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO send_feedback(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO viewresponse(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO SummaryReport(ISM_PlannerReportsDTO data);

        ISM_PlannerReportsDTO getcatempdetails(ISM_PlannerReportsDTO data);

        Task<ISM_PlannerReportsDTO> getempcatedetails(ISM_PlannerReportsDTO data);

        ISM_PlannerReportsDTO saveAddLop(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO deactivate(ISM_PlannerReportsDTO data);
        ISM_PlannerReportsDTO getonlodatdata(ISM_PlannerReportsDTO data);

        ISM_PlannerReportsDTO getconcalllopdetails(ISM_PlannerReportsDTO data);

    }
}

