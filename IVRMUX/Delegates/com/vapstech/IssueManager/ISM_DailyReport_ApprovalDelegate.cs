using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_DailyReport_ApprovalDelegate
    {
        CommonDelegate<ISM_DailyReport_ApprovalDTO, ISM_DailyReport_ApprovalDTO> COMMM = new CommonDelegate<ISM_DailyReport_ApprovalDTO, ISM_DailyReport_ApprovalDTO>();

        public ISM_DailyReport_ApprovalDTO GetDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/GetDetails/");
        }
        public ISM_DailyReport_ApprovalDTO DeptSelectionDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/DeptSelectionDetails/");
        }
        public ISM_DailyReport_ApprovalDTO DesgSelectionDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/DesgSelectionDetails/");
        }
        public ISM_DailyReport_ApprovalDTO GetSavedDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/GetSavedDetails/");
        }
        public ISM_DailyReport_ApprovalDTO SaveApprovalDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/SaveApprovalDetails/");
        }
        public ISM_DailyReport_ApprovalDTO ViewCheckListSavedDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/ViewCheckListSavedDetails/");
        }
        public ISM_DailyReport_ApprovalDTO viewresponse(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/viewresponse/");
        }

        /* DR Not Sent Remrks Report Details */
        public ISM_DailyReport_ApprovalDTO GetDRRemarksReport(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/GetDRRemarksReport/");
        }

        /* DR Not Approved Details */
        public ISM_DailyReport_ApprovalDTO GetDRNotApprovedList(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/GetDRNotApprovedList/");
        }

        /* DR Multi Approval Details */
        public ISM_DailyReport_ApprovalDTO GetMultiDRSavedDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/GetMultiDRSavedDetails/");
        }
        public ISM_DailyReport_ApprovalDTO ViewApprovalTaskDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/ViewApprovalTaskDetails/");
        }
        public ISM_DailyReport_ApprovalDTO SaveDRMultiApprovalDetails(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/SaveDRMultiApprovalDetails/");
        }
        public ISM_DailyReport_ApprovalDTO ViewOverAllComments(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/ViewOverAllComments/");
        }
        public ISM_DailyReport_ApprovalDTO SaveRegenerateDRApproval(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/SaveRegenerateDRApproval/");
        }
        public ISM_DailyReport_ApprovalDTO GetSavedDetails1(ISM_DailyReport_ApprovalDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_DailyReport_ApprovalFacade/GetSavedDetails1/");
        }
    }
}