using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_NonEarnedReportInterface
    {
        ISM_NonEarnedReportDTO getalldata(ISM_NonEarnedReportDTO data);
        ISM_NonEarnedReportDTO Deptselectiondetails(ISM_NonEarnedReportDTO data);
        ISM_NonEarnedReportDTO Desgselectiondetails(ISM_NonEarnedReportDTO data);
        Task<ISM_NonEarnedReportDTO> getempblockReport(ISM_NonEarnedReportDTO data);
    }
}
