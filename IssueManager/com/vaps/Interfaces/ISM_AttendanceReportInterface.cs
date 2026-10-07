using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface ISM_AttendanceReportInterface
    {
        Task<ISM_AttendanceReportsDTO> getdetails(ISM_AttendanceReportsDTO data);
        Task<ISM_AttendanceReportsDTO> getplannerReport(ISM_AttendanceReportsDTO data);
    }
}
