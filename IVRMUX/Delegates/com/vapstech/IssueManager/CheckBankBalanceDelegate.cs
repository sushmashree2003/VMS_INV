using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.IssueManager
{
    public class CheckBankBalanceDelegate
    {
        CommonDelegate<CheckBankBalanceDTO, CheckBankBalanceDTO> comm = new CommonDelegate<CheckBankBalanceDTO, CheckBankBalanceDTO>();
        //
        CommonDelegate<VMSMaster_ChequeBookdto, VMSMaster_ChequeBookdto> commT = new CommonDelegate<VMSMaster_ChequeBookdto, VMSMaster_ChequeBookdto>();
        public CheckBankBalanceDTO loaddata(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/loaddata");
        }
        public CheckBankBalanceDTO savedata(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/savedata");
        }
        //GetReport
        public CheckBankBalanceDTO getReporttwo(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/getReporttwo");
        }
        //ClearenceDate
        public CheckBankBalanceDTO ClearenceDate(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/ClearenceDate");
        }
        public CheckBankBalanceDTO Editdata(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/Editdata");
        }
        public CheckBankBalanceDTO clientDecative(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/clientDecative");
        }
        public VMSMaster_ChequeBookdto getProject(VMSMaster_ChequeBookdto data)
        {
            return commT.POSTIssueManager(data, "CheckBankBalanceFacade/getProject");
        }
        public CheckBankBalanceDTO getbom(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/getbom");
        }
        public CheckBankBalanceDTO companychange(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/companychange");
        }
        public VMSMaster_ChequeBookdto viewdetails(VMSMaster_ChequeBookdto data)
        {
            return commT.POSTIssueManager(data, "CheckBankBalanceFacade/viewdetails");
        }
        public CheckBankBalanceDTO getinvoiceno(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/getinvoiceno");
        }
        public VMSMaster_ChequeBookdto savepaymentdetailsrecord(VMSMaster_ChequeBookdto data)
        {
            return commT.POSTIssueManager(data, "CheckBankBalanceFacade/savepaymentdetailsrecord");
        }
        public VMSMaster_ChequeBookdto loaddataconfig(VMSMaster_ChequeBookdto data)
        {
            return commT.POSTIssueManager(data, "CheckBankBalanceFacade/loaddataconfig");
        }
        public CheckBankBalanceDTO savedataconfig(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/savedataconfig");
        }
        public CheckBankBalanceDTO loaddatareport(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/loaddatareport");
        }
        public CheckBankBalanceDTO getreport(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/getreport");
        }
        //paymentnotification
        public CheckBankBalanceDTO paymentnotification(CheckBankBalanceDTO data)
        {
            return comm.POSTIssueManager(data, "CheckBankBalanceFacade/paymentnotification");
        }
    }
}
