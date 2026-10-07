using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class CheckBookLogReportDelegate
    {
        CommonDelegate<CheckBookLogReportDTO, CheckBookLogReportDTO> comm = new CommonDelegate<CheckBookLogReportDTO, CheckBookLogReportDTO>();
       
        public CheckBookLogReportDTO loaddata(CheckBookLogReportDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookLogReportFacade/loaddata");
        }

        public CheckBookLogReportDTO onchange(CheckBookLogReportDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookLogReportFacade/onchange");
        }
        public CheckBookLogReportDTO Report(CheckBookLogReportDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBookLogReportFacade/Report");
        }
    }
}
