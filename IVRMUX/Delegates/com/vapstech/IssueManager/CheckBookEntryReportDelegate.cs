using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class CheckBookEntryReportDelegate
    {
        CommonDelegate<CheckBookEntryReportDTO, CheckBookEntryReportDTO> COMINV = new CommonDelegate<CheckBookEntryReportDTO, CheckBookEntryReportDTO>();
        public CheckBookEntryReportDTO getloaddata(CheckBookEntryReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "CheckBookEntryReportFacade/getloaddata/");
        }
        public CheckBookEntryReportDTO onreport(CheckBookEntryReportDTO data)
        {
            return COMINV.POSTIssueManager(data, "CheckBookEntryReportFacade/onreport/");
        }

        
    }
}
