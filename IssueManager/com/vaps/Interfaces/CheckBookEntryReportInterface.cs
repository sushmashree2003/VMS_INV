using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface CheckBookEntryReportInterface
    {
        CheckBookEntryReportDTO getloaddata(CheckBookEntryReportDTO data);
        CheckBookEntryReportDTO onreport(CheckBookEntryReportDTO data);
      
    }
}
