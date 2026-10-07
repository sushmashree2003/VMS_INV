using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PlannerDeviationReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO> COMMM = new CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO>();

        public ISM_PlannerReportsDTO getdetails(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeviationReportFacade/getdetails/");
        }
        public ISM_PlannerReportsDTO getdeviationReport(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeviationReportFacade/getdeviationReport");
        }

        //======================= weekly deviation
        public ISM_PlannerReportsDTO getdetails_weekly(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeviationReportFacade/getdetails_weekly/");
        }
        public ISM_PlannerReportsDTO getdeviationReport_weekly(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeviationReportFacade/getdeviationReport_weekly");
        }

        //=====================rating report==========
        public ISM_PlannerReportsDTO getEmp_ratingReport(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/getEmp_ratingReport");
        }
        public ISM_PlannerReportsDTO taskRejectReport(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/taskRejectReport");
        }
        

    }
}
