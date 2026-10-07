using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_ConsolidateDailyReportInterface
    {
        Task<ISM_DailyReportGenerationDTO> getdetails(ISM_DailyReportGenerationDTO data);
        Task<ISM_DailyReportGenerationDTO> getReport(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO ViewCheckListSavedDetails(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO viewresponse(ISM_DailyReportGenerationDTO data);
        Task<ISM_DailyReportGenerationDTO> getfeedbackReport(ISM_DailyReportGenerationDTO data);      
        Task<ISM_DailyReportGenerationDTO> viewcomments(ISM_DailyReportGenerationDTO data);      
        ISM_DailyReportGenerationDTO getmodulelist(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO getcomplinetstaskreport(ISM_DailyReportGenerationDTO data);      
        ISM_DailyReportGenerationDTO getyerlyplannerreport(ISM_DailyReportGenerationDTO data);      
    }
}
