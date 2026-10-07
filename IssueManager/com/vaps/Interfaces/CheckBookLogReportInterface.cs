 using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface CheckBookLogReportInterface
    {
        CheckBookLogReportDTO loaddata(CheckBookLogReportDTO data);
        CheckBookLogReportDTO onchange(CheckBookLogReportDTO data);
        CheckBookLogReportDTO Report(CheckBookLogReportDTO data);


    }
}
