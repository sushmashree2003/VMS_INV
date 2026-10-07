using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class VMS_Receipt_VoucherDelegate
    {
        CommonDelegate<VMS_Receipt_VoucherDTO, VMS_Receipt_VoucherDTO> comm = new CommonDelegate<VMS_Receipt_VoucherDTO, VMS_Receipt_VoucherDTO>();
        CommonDelegate<CheckBookEntryDTO, CheckBookEntryDTO> commnn = new CommonDelegate<CheckBookEntryDTO, CheckBookEntryDTO>();
        
        public VMS_Receipt_VoucherDTO loaddata(VMS_Receipt_VoucherDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Receipt_VoucherFacade/loaddata");
        }
        public VMS_Receipt_VoucherDTO savedata(VMS_Receipt_VoucherDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Receipt_VoucherFacade/savedata");
        }
        //onchange
        public VMS_Receipt_VoucherDTO onchange(VMS_Receipt_VoucherDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Receipt_VoucherFacade/onchange");
        }
        //savedataSource
        public CheckBookEntryDTO savedataSource(CheckBookEntryDTO data)
        {
            return commnn.POSTIssueManager(data, "VMS_Receipt_VoucherFacade/savedataSource");
        }
        //clientDecative
        public CheckBookEntryDTO clientDecative(CheckBookEntryDTO data)
        {
            return commnn.POSTIssueManager(data, "VMS_Receipt_VoucherFacade/clientDecative");
        }

        //
        public VMS_Receipt_VoucherDTO RecieptReport(VMS_Receipt_VoucherDTO data)
        {
            return comm.POSTIssueManager(data, "VMS_Receipt_VoucherFacade/RecieptReport");
        }
    }
}
