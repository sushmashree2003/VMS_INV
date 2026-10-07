using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface CheckBankBalanceInterface
    {
        CheckBankBalanceDTO loaddata(CheckBankBalanceDTO data);
        CheckBankBalanceDTO savedata(CheckBankBalanceDTO data);
        CheckBankBalanceDTO Editdata(CheckBankBalanceDTO data);
        CheckBankBalanceDTO getinvoiceno(CheckBankBalanceDTO data);
        CheckBankBalanceDTO clientDecative(CheckBankBalanceDTO data);
        CheckBankBalanceDTO getbom(CheckBankBalanceDTO data);
        CheckBankBalanceDTO companychange(CheckBankBalanceDTO data);
        VMSMaster_ChequeBookdto getProject(VMSMaster_ChequeBookdto data);
        VMSMaster_ChequeBookdto viewdetails(VMSMaster_ChequeBookdto data);
        VMSMaster_ChequeBookdto savepaymentdetailsrecord(VMSMaster_ChequeBookdto data);
        VMSMaster_ChequeBookdto loaddataconfig(VMSMaster_ChequeBookdto data);
        CheckBankBalanceDTO savedataconfig(CheckBankBalanceDTO data);
        CheckBankBalanceDTO loaddatareport(CheckBankBalanceDTO data);
        CheckBankBalanceDTO getreport(CheckBankBalanceDTO data);
        CheckBankBalanceDTO paymentnotification(CheckBankBalanceDTO data);
        //CheckBankBalanceDTO getYear(CheckBankBalanceDTO data);
        //GetReport
        CheckBankBalanceDTO getReporttwo(CheckBankBalanceDTO data);
        //ClearenceDate
        CheckBankBalanceDTO ClearenceDate(CheckBankBalanceDTO data);
    }
}
