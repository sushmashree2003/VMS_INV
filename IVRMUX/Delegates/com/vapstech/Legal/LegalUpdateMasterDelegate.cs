using CommonLibrary;
using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Legal
{
    public class LegalUpdateMasterDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Legal_CaseDetailsDTO, Legal_CaseDetailsDTO> COMMM = new CommonDelegate<Legal_CaseDetailsDTO, Legal_CaseDetailsDTO>();

        public Legal_CaseDetailsDTO savelegalcasedetails(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/savelegalcasedetails/");
        }

        public Legal_CaseDetailsDTO savecasestatus(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/savecasestatus/");
        }

        public Legal_CaseDetailsDTO saveadvocate(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/saveadvocate/");
        }

        public Legal_CaseDetailsDTO saveacts(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/saveacts/");
        }

        public Legal_CaseDetailsDTO savecasehistory(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/savecasehistory/");
        }

        public Legal_CaseDetailsDTO savefinalorder(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/savefinalorder/");
        }

        public Legal_CaseDetailsDTO SaveDocuments(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/SaveDocuments/");
        }

        public Legal_CaseDetailsDTO loaddata(Legal_CaseDetailsDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "LegalUpdateMasterFacade/loaddata");
        }

        public Legal_CaseDetailsDTO ViewLegalCaseDetails(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/ViewLegalCaseDetails/");
        }

        public Legal_CaseDetailsDTO editdata(Legal_CaseDetailsDTO data)
        {
            return COMMM.POSTIssueManager(data, "LegalUpdateMasterFacade/editdata/");
        }
    }
}
