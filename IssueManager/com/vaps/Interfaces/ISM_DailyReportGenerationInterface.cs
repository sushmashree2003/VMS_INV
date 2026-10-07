using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_DailyReportGenerationInterface
    {
        ISM_DailyReportGenerationDTO loaddata(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO generatedailyreport(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO searchpreviousdailyreport(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO addextratask(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO viewresponse(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO Otherdaycount(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO SaveRemarks(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO SaveDRNotSentRemarks(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO CheckDeviationRemarks(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO ViewCheckListSavedDetails(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO ViewBlockedDetails(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO GetCategoryChecklistDetails(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO ViewDRNotApprovedList(ISM_DailyReportGenerationDTO data);

        ISM_DailyReportGenerationDTO TriggerPlannerMail(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO DRGenerationreject(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO generateRejectedtask(ISM_DailyReportGenerationDTO data);
        ISM_DailyReportGenerationDTO saveDRremarks(ISM_DailyReportGenerationDTO data);
    }
}
