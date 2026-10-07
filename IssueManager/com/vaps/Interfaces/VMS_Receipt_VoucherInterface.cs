using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface VMS_Receipt_VoucherInterface
    {
        VMS_Receipt_VoucherDTO loaddata(VMS_Receipt_VoucherDTO data);
        VMS_Receipt_VoucherDTO savedata(VMS_Receipt_VoucherDTO data);
        //onchange
        VMS_Receipt_VoucherDTO onchange(VMS_Receipt_VoucherDTO data);

        //CheckBookEntryDTO
        CheckBookEntryDTO savedataSource(CheckBookEntryDTO data);
        //clientDecative
        CheckBookEntryDTO clientDecative(CheckBookEntryDTO data);
        //RecieptReport
        VMS_Receipt_VoucherDTO RecieptReport(VMS_Receipt_VoucherDTO data);
    }
}
