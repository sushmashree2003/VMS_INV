using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public  interface ISM_PrinterStatisticsInterface
    {
        Task<ISM_PrinterStatisticsDTO> getdetails(ISM_PrinterStatisticsDTO data);
        
        ISM_PrinterStatisticsDTO savedata(ISM_PrinterStatisticsDTO data);
        ISM_PrinterStatisticsDTO deactive(ISM_PrinterStatisticsDTO data);
        ISM_PrinterStatisticsDTO gettaskAttachment(ISM_PrinterStatisticsDTO data);

        ISM_PrinterStatisticsDTO PrinterEmail(ISM_PrinterStatisticsDTO data);

        ISM_PrinterStatisticsDTO deptchange(ISM_PrinterStatisticsDTO data);

    }
}
