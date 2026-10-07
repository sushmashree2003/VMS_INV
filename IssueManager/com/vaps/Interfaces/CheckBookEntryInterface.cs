using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface CheckBookEntryInterface
    {
        CheckBookEntryDTO loaddata(CheckBookEntryDTO data);
        CheckBookEntryDTO savedata(CheckBookEntryDTO data);
        // savedataSource
        CheckBookEntryDTO savedataSource(CheckBookEntryDTO data);
        CheckBookEntryDTO Editdata(CheckBookEntryDTO data);
        CheckBookEntryDTO getinvoiceno(CheckBookEntryDTO data);
        CheckBookEntryDTO clientDecative(CheckBookEntryDTO data);
        CheckBookEntryDTO getbom(CheckBookEntryDTO data);
        CheckBookEntryDTO companychange(CheckBookEntryDTO data);
        CheckBookEntryDTO getProject(CheckBookEntryDTO data);
        CheckBookEntryDTO viewdetails(CheckBookEntryDTO data);
        CheckBookEntryDTO sms(CheckBookEntryDTO data);
        CheckBookEntryDTO loaddataconfig(CheckBookEntryDTO data);
        CheckBookEntryDTO savedataconfig(CheckBookEntryDTO data);
        CheckBookEntryDTO loaddatareport(CheckBookEntryDTO data);
        CheckBookEntryDTO getreport(CheckBookEntryDTO data);
        CheckBookEntryDTO paymentnotification(CheckBookEntryDTO data);
        CheckBookEntryDTO savedataEMI(CheckBookEntryDTO data);
        CheckBookEntryDTO geterifyEmail(CheckBookEntryDTO data);
        CheckBookEntryDTO viewuploadflies(CheckBookEntryDTO data);
    }
}
