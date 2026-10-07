using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PrinterStatisticsReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PrinterStatisticsDTO, ISM_PrinterStatisticsDTO> COMMM = new CommonDelegate<ISM_PrinterStatisticsDTO, ISM_PrinterStatisticsDTO>();

        public ISM_PrinterStatisticsDTO getdetails(ISM_PrinterStatisticsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsReportFacade/getdetails/");
        }
        public ISM_PrinterStatisticsDTO get_report(ISM_PrinterStatisticsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsReportFacade/get_report");
        }
        public ISM_PrinterStatisticsDTO sendmail(ISM_PrinterStatisticsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsReportFacade/sendmail/");
        }
    }
}
