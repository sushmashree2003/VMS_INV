using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_DailyReportGeneratioDelegate
    {
        CommonDelegate<ISM_DailyReportGenerationDTO, ISM_DailyReportGenerationDTO> _comm = new CommonDelegate<ISM_DailyReportGenerationDTO, ISM_DailyReportGenerationDTO>();

        public ISM_DailyReportGenerationDTO loaddata(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/loaddata");
        }
        public ISM_DailyReportGenerationDTO generatedailyreport(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/generatedailyreport");
        }
        public ISM_DailyReportGenerationDTO searchpreviousdailyreport(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/searchpreviousdailyreport");
        }
        public ISM_DailyReportGenerationDTO addextratask(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/addextratask");
        }
        public ISM_DailyReportGenerationDTO viewresponse(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/viewresponse");
        }
        public ISM_DailyReportGenerationDTO Otherdaycount(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/Otherdaycount");
        }  
        public ISM_DailyReportGenerationDTO SaveRemarks(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/SaveRemarks");
        }  
        public ISM_DailyReportGenerationDTO SaveDRNotSentRemarks(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/SaveDRNotSentRemarks");
        }  
        public ISM_DailyReportGenerationDTO CheckDeviationRemarks(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/CheckDeviationRemarks");
        }   
        public ISM_DailyReportGenerationDTO ViewCheckListSavedDetails(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/ViewCheckListSavedDetails");
        }     
        public ISM_DailyReportGenerationDTO ViewBlockedDetails(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/ViewBlockedDetails");
        }     
        public ISM_DailyReportGenerationDTO GetCategoryChecklistDetails(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/GetCategoryChecklistDetails");
        }      
        public ISM_DailyReportGenerationDTO ViewDRNotApprovedList(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/ViewDRNotApprovedList");
        }
        public ISM_DailyReportGenerationDTO DRGenerationreject(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/DRGenerationreject");
        }
        public ISM_DailyReportGenerationDTO generateRejectedtask(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/generateRejectedtask");
        }
        public ISM_DailyReportGenerationDTO saveDRremarks(ISM_DailyReportGenerationDTO data)
        {
            return _comm.POSTIssueManager(data, "ISM_DailyReportGenerationFacade/saveDRremarks");
        }
    }
}