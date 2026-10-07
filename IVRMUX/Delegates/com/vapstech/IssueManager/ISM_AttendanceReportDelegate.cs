using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_AttendanceReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_AttendanceReportsDTO, ISM_AttendanceReportsDTO> COMMM = new CommonDelegate<ISM_AttendanceReportsDTO, ISM_AttendanceReportsDTO>();
        public ISM_AttendanceReportsDTO getdetails(ISM_AttendanceReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_AttendanceReportFacade/getdetails");
        }
        public ISM_AttendanceReportsDTO getplannerReport(ISM_AttendanceReportsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_AttendanceReportFacade/getplannerReport");
        }
    }
}
