using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_SwapIssueReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO> COMMM = new CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO>();

        public ISM_PlannerReportsDTO getdetails(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_SwapIssueReportFacade/getdetails/");
        }
        public ISM_PlannerReportsDTO getdeviationReport(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_SwapIssueReportFacade/getdeviationReport");
        }

        //======================= weekly deviation
        public ISM_PlannerReportsDTO getdetails_weekly(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_SwapIssueReportFacade/getdetails_weekly/");
        }
        public ISM_PlannerReportsDTO getdeviationReport_weekly(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_SwapIssueReportFacade/getdeviationReport_weekly");
        }

        //=====================rating report==========
       
    }
}
