using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_DailyReport_ApprovalInterface
    {
        ISM_DailyReport_ApprovalDTO GetDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO DeptSelectionDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO DesgSelectionDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO GetSavedDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO SaveApprovalDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO ViewCheckListSavedDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO viewresponse(ISM_DailyReport_ApprovalDTO data);

        /* DR Not Sent Remrks Report Details */
        ISM_DailyReport_ApprovalDTO GetDRRemarksReport(ISM_DailyReport_ApprovalDTO data);

        /* DR Not Approved Details */
        ISM_DailyReport_ApprovalDTO GetDRNotApprovedList(ISM_DailyReport_ApprovalDTO data);

        /* DR Multi Approved Details */
        ISM_DailyReport_ApprovalDTO GetMultiDRSavedDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO ViewApprovalTaskDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO SaveDRMultiApprovalDetails(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO ViewOverAllComments(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO SaveRegenerateDRApproval(ISM_DailyReport_ApprovalDTO data);
        ISM_DailyReport_ApprovalDTO GetSavedDetails1(ISM_DailyReport_ApprovalDTO data);

    }
}
