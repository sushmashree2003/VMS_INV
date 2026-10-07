using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class EmailSms_HistoryDelegate
    {

        CommonDelegate<EmailSms_HistoryDTO, EmailSms_HistoryDTO> _comm = new CommonDelegate<EmailSms_HistoryDTO, EmailSms_HistoryDTO>();
        public EmailSms_HistoryDTO getdetails(EmailSms_HistoryDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailSms_HistoryReportFacade/getdetails");
        }
        public EmailSms_HistoryDTO getmodule(EmailSms_HistoryDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailSms_HistoryReportFacade/getmodule");
        }
        //
        public EmailSms_HistoryDTO getReport(EmailSms_HistoryDTO data)
        {
            return _comm.POSTIssueManager(data, "EmailSms_HistoryReportFacade/getReport");
        }
    }
}
