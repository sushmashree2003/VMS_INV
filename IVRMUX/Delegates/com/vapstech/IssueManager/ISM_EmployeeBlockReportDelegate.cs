using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_EmployeeBlockReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_BlockEmployeeDTO, ISM_BlockEmployeeDTO> COMMM = new CommonDelegate<ISM_BlockEmployeeDTO, ISM_BlockEmployeeDTO>();

      
        public ISM_BlockEmployeeDTO getempblockReport(ISM_BlockEmployeeDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_EmployeeBlockReportFacade/getempblockReport");
        }
        //getempblockNewReport
        public ISM_BlockEmployeeDTO getempblockNewReport(ISM_BlockEmployeeDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_EmployeeBlockReportFacade/getempblockNewReport");
        }


    }
}
