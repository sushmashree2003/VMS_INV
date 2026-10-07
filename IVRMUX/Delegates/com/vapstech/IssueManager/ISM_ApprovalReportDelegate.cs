using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_ApprovalReportDelegate
    {
        CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO> _comm = new CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO>();
        public ISM_PlannerReportsDTO getdetails(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getdetails");
        }
        public ISM_PlannerReportsDTO getPlannerlist(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getPlannerlist");
        }
        public ISM_PlannerReportsDTO getApprovalReport(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getApprovalReport");
        }
        public ISM_PlannerReportsDTO getPlannerDReport(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getPlannerDReport");
        }
        public ISM_PlannerReportsDTO Deptselectiondetails(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/Deptselectiondetails");
        }
        public ISM_PlannerReportsDTO Desgselectiondetails(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/Desgselectiondetails");
        }
        public ISM_PlannerReportsDTO getempdetailsReport(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getempdetailsReport");
        }
        public ISM_PlannerReportsDTO send_feedback(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/send_feedback");
        }
        public ISM_PlannerReportsDTO viewresponse(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/viewresponse");
        }
        public ISM_PlannerReportsDTO SummaryReport(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/SummaryReport");
        }

        public ISM_PlannerReportsDTO getcatempdetails(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getcatempdetails");
        }
        public ISM_PlannerReportsDTO getempcatedetails(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getempcatedetails");
        }

        public ISM_PlannerReportsDTO saveAddLop(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/saveAddLop");
        }
        public ISM_PlannerReportsDTO deactivate(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/deactivate");
        }
        public ISM_PlannerReportsDTO getonlodatdata(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getonlodatdata");
        }

        public ISM_PlannerReportsDTO getconcalllopdetails(ISM_PlannerReportsDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_ApprovalReportFacade/getconcalllopdetails");
        }

    }
}
