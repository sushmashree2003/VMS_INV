using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface LegalUpdateMasterInterface
    {
        Legal_CaseDetailsDTO savelegalcasedetails(Legal_CaseDetailsDTO dto);
        Legal_CaseDetailsDTO savecasestatus(Legal_CaseDetailsDTO dto);
        Legal_CaseDetailsDTO saveadvocate(Legal_CaseDetailsDTO dto);
        Legal_CaseDetailsDTO saveacts(Legal_CaseDetailsDTO dto);
        Legal_CaseDetailsDTO savecasehistory(Legal_CaseDetailsDTO dto);
        Legal_CaseDetailsDTO savefinalorder(Legal_CaseDetailsDTO dto);
        Legal_CaseDetailsDTO SaveDocuments(Legal_CaseDetailsDTO dto);

        Legal_CaseDetailsDTO loaddata(Legal_CaseDetailsDTO dto);

        Legal_CaseDetailsDTO ViewLegalCaseDetails(Legal_CaseDetailsDTO data);
        Legal_CaseDetailsDTO editdata(Legal_CaseDetailsDTO data);
    }
}
