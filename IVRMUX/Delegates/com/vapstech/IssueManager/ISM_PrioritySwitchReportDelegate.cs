using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PrioritySwitchReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PrioritySwitchReportDTO, ISM_PrioritySwitchReportDTO> COMMM = new CommonDelegate<ISM_PrioritySwitchReportDTO, ISM_PrioritySwitchReportDTO>();

        public ISM_PrioritySwitchReportDTO getdetails(ISM_PrioritySwitchReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrioritySwitchReportFacade/getdetails/");
        }
        public ISM_PrioritySwitchReportDTO getdeviationReport(ISM_PrioritySwitchReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrioritySwitchReportFacade/getdeviationReport");
        }

        //======================= weekly deviation
        public ISM_PrioritySwitchReportDTO getdetails_weekly(ISM_PrioritySwitchReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrioritySwitchReportFacade/getdetails_weekly/");
        }
        public ISM_PrioritySwitchReportDTO getdeviationReport_weekly(ISM_PrioritySwitchReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrioritySwitchReportFacade/getdeviationReport_weekly");
        }

        //=====================rating report==========
        public ISM_PrioritySwitchReportDTO getEmp_ratingReport(ISM_PrioritySwitchReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerDeptDeviationReportFacade/getEmp_ratingReport");
        }


    }
}
