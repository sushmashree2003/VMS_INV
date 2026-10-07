using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_EmployeeBlockReportInterface
    {       
        Task<ISM_BlockEmployeeDTO> getempblockReport(ISM_BlockEmployeeDTO data);
        //getempblockNewReport
        Task<ISM_BlockEmployeeDTO> getempblockNewReport(ISM_BlockEmployeeDTO data);
    }
}
