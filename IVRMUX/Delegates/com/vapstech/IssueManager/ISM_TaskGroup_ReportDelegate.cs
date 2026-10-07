using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_TaskGroup_ReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_TaskGroup_ReportDTO, ISM_TaskGroup_ReportDTO> COMMM = new CommonDelegate<ISM_TaskGroup_ReportDTO, ISM_TaskGroup_ReportDTO>();
        public ISM_TaskGroup_ReportDTO getdetails(ISM_TaskGroup_ReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskGroup_ReportFacade/getdetails");
        }
        //public ISM_TaskGroup_ReportDTO getdepartments(ISM_TaskGroup_ReportDTO data)
        //{
        //    return COMMM.POSTIssueManager(data, "ISM_TaskGroup_ReportFacade/getdepartments");
        //}
        public ISM_TaskGroup_ReportDTO getplannerReport(ISM_TaskGroup_ReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskGroup_ReportFacade/getplannerReport");
        }
        public ISM_TaskGroup_ReportDTO getReport(ISM_TaskGroup_ReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskGroup_ReportFacade/getReport");
        }

        /* CATEGORY GROUP  */
        public ISM_TaskGroup_ReportDTO getcategorydetails(ISM_TaskGroup_ReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskGroup_ReportFacade/getcategorydetails");
        }
        public ISM_TaskGroup_ReportDTO onchangedept(ISM_TaskGroup_ReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskGroup_ReportFacade/onchangedept");
        }
        public ISM_TaskGroup_ReportDTO categorygroupReport(ISM_TaskGroup_ReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskGroup_ReportFacade/categorygroupReport");
        }
        //SaveFeedBack
        public ISM_TaskGroup_ReportDTO SaveFeedBack(ISM_TaskGroup_ReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_TaskGroup_ReportFacade/SaveFeedBack");
        }
    }
}
