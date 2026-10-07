using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface CheckBookPrintInterface
    {
        CheckBookPrintDTO loaddata(CheckBookPrintDTO data);
        CheckBookPrintDTO savedata(CheckBookPrintDTO data);
        CheckBookPrintDTO Editdata(CheckBookPrintDTO data);
        CheckBookPrintDTO getinvoiceno(CheckBookPrintDTO data);
        CheckBookPrintDTO clientDecative(CheckBookPrintDTO data);
        CheckBookPrintDTO getbom(CheckBookPrintDTO data);
        CheckBookPrintDTO companychange(CheckBookPrintDTO data);
        CheckBookPrintDTO getProject(CheckBookPrintDTO data);
        CheckBookPrintDTO viewdetails(CheckBookPrintDTO data);
        CheckBookPrintDTO savepaymentdetailsrecord(CheckBookPrintDTO data);
        CheckBookPrintDTO loaddataconfig(CheckBookPrintDTO data);
        CheckBookPrintDTO savedataconfig(CheckBookPrintDTO data);
        CheckBookPrintDTO loaddatareport(CheckBookPrintDTO data);
        CheckBookPrintDTO getreport(CheckBookPrintDTO data);
        CheckBookPrintDTO paymentnotification(CheckBookPrintDTO data);
        //CheckBookPrintDTO getYear(CheckBookPrintDTO data);
    }
}
