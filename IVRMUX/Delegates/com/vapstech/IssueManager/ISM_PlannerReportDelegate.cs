using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PlannerReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO> COMMM = new CommonDelegate<ISM_PlannerReportsDTO, ISM_PlannerReportsDTO>();
        public ISM_PlannerReportsDTO getdetails(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerReportFacade/getdetails");
        }
        public ISM_PlannerReportsDTO getplannerReport(ISM_PlannerReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PlannerReportFacade/getplannerReport");
        }
    }
}
