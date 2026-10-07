using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface EmailSms_HistoryReportInterface
    {
        EmailSms_HistoryDTO getdetails(EmailSms_HistoryDTO data);
        //getmodule
        EmailSms_HistoryDTO getmodule(EmailSms_HistoryDTO data);
        //
        EmailSms_HistoryDTO getReport(EmailSms_HistoryDTO data);
    }
}
