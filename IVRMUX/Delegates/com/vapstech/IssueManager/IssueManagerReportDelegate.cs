using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class IssueManagerReportDelegate
    {

        CommonDelegate<IssueManagerReportDTO, IssueManagerReportDTO> _comm = new CommonDelegate<IssueManagerReportDTO, IssueManagerReportDTO>();
        public IssueManagerReportDTO getdetails(IssueManagerReportDTO data)
        {
            return _comm.POSTIssueManager(data, "IssueManagerReportFACADE/getdetails");
        }
                //
        public IssueManagerReportDTO getReport(IssueManagerReportDTO data)
        {
            return _comm.POSTIssueManager(data, "IssueManagerReportFACADE/getReport");
        }
    }
}
