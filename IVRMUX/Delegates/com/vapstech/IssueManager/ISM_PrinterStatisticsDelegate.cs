using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;


namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class ISM_PrinterStatisticsDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ISM_PrinterStatisticsDTO, ISM_PrinterStatisticsDTO> COMMM = new CommonDelegate<ISM_PrinterStatisticsDTO, ISM_PrinterStatisticsDTO>();

        public ISM_PrinterStatisticsDTO getdetails(ISM_PrinterStatisticsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsFacade/getdetails/");
        }
        //public ISM_PrinterStatisticsDTO getdetails(ISM_PrinterStatisticsDTO data)
        //{
        //    return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsFacade/getdetails/");
        //}
        public ISM_PrinterStatisticsDTO savedata(ISM_PrinterStatisticsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsFacade/savedata");
        }
        public ISM_PrinterStatisticsDTO deactive(ISM_PrinterStatisticsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsFacade/deactive");
        }
        public ISM_PrinterStatisticsDTO gettaskAttachment(ISM_PrinterStatisticsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsFacade/gettaskAttachment");
        }
        public ISM_PrinterStatisticsDTO deptchange(ISM_PrinterStatisticsDTO data)
        {
            return COMMM.POSTIssueManager(data, "ISM_PrinterStatisticsFacade/deptchange");
        }
    }
}
