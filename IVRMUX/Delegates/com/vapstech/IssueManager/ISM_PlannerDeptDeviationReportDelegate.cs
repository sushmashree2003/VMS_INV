using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PlannerDeptDeviationReportDelegate 
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO> COMMM = new CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO>();

        public ISM_PlannerReportsDTO getdetails(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/getdetails/");
        }
        public ISM_PlannerReportsDTO getdeviationReport(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/getdeviationReport");
        }

        public ISM_PlannerReportsDTO Deptselectiondetails(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/Deptselectiondetails");
        }
        public ISM_PlannerReportsDTO DeptselectionHeaddetails(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/DeptselectionHeaddetails");
        }
        public ISM_PlannerReportsDTO gettaskdetails(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/gettaskdetails");
        }
        public ISM_PlannerReportsDTO gettaskdetailsreport(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/gettaskdetailsreport");
        }
        public ISM_PlannerReportsDTO Desgselectiondetails(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/Desgselectiondetails");
        }
        public ISM_PlannerReportsDTO empcommunication(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/empcommunication");
        }

        //======================= weekly deviation
        public ISM_PlannerReportsDTO getdetails_weekly(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/getdetails_weekly/");
        }
        public ISM_PlannerReportsDTO getdeviationReport_weekly(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/getdeviationReport_weekly");
        }
        //====================dailydeviation
        public ISM_PlannerReportsDTO getdaily_deviation(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/getdaily_deviation");
        }
       
    }
}
