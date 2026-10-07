using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface ISM_PrinterStatisticsReportInterface
    {
        Task<ISM_PrinterStatisticsDTO> getdetails(ISM_PrinterStatisticsDTO data);
        Task<ISM_PrinterStatisticsDTO> get_report(ISM_PrinterStatisticsDTO data);
        Task<ISM_PrinterStatisticsDTO> sendmail(ISM_PrinterStatisticsDTO data);
    }
}
