using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_ConsolidateDailyReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_DailyReportGenerationDTO, ISM_DailyReportGenerationDTO> COMMM = new CommonDelegate<ISM_DailyReportGenerationDTO, ISM_DailyReportGenerationDTO>();

        public ISM_DailyReportGenerationDTO getdetails(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/getdetails/");
        }
        public ISM_DailyReportGenerationDTO getReport(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/getReport");
        }
        public ISM_DailyReportGenerationDTO ViewCheckListSavedDetails(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/ViewCheckListSavedDetails");
        }
        public ISM_DailyReportGenerationDTO viewresponse(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/viewresponse");
        }
        public ISM_DailyReportGenerationDTO getfeedbackReport(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/getfeedbackReport");
        }
        public ISM_DailyReportGenerationDTO viewcomments(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/viewcomments");
        }
        public ISM_DailyReportGenerationDTO getmodulelist(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/getmodulelist");
        }
        public ISM_DailyReportGenerationDTO getcomplinetstaskreport(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/getcomplinetstaskreport");
        }
        public ISM_DailyReportGenerationDTO getyerlyplannerreport(ISM_DailyReportGenerationDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_ConsolidateDailyReportFacade/getyerlyplannerreport");
        }
    }
}
