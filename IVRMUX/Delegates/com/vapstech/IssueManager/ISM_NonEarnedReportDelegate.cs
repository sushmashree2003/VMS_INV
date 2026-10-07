using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_NonEarnedReportDelegate
    {
        CommonDelegate<ISM_NonEarnedReportDTO, ISM_NonEarnedReportDTO> COMMM = new CommonDelegate<ISM_NonEarnedReportDTO, ISM_NonEarnedReportDTO>();

        public ISM_NonEarnedReportDTO getalldata(ISM_NonEarnedReportDTO id)
        {
            return COMMM.POSTIssueManager(id, "ISM_NonEarnedReportFacade/getalldata/");
        }
        //Deptselectiondetails
        public ISM_NonEarnedReportDTO Deptselectiondetails(ISM_NonEarnedReportDTO id)
        {
            return COMMM.POSTIssueManager(id, "ISM_NonEarnedReportFacade/Deptselectiondetails/");
        }
        //Desgselectiondetails
        public ISM_NonEarnedReportDTO Desgselectiondetails(ISM_NonEarnedReportDTO id)
        {
            return COMMM.POSTIssueManager(id, "ISM_NonEarnedReportFacade/Desgselectiondetails/");
        }
        public ISM_NonEarnedReportDTO getempblockReport(ISM_NonEarnedReportDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_NonEarnedReportFacade/getempblockReport");
        }

    }
}