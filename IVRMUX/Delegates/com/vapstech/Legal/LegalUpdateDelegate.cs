using CommonLibrary;
using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Legal
{
    public class LegalUpdateDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LegalUpdateDTO, LegalUpdateDTO> COMMM = new CommonDelegate<LegalUpdateDTO, LegalUpdateDTO>();

        //CommonDelegate<LegalUpdateDTO, LegalUpdateDTO> COMMNFUN = new CommonDelegate<LegalUpdateDTO, LegalUpdateDTO>();

        // ////////////////////////////////trans//////////////////////////////////////////////

        public LegalUpdateDTO loaddata(LegalUpdateDTO dto)
        {
            return COMMM.POSTIssueManager(dto, "LegalUpdateFacade/loaddata");
        }

        public LegalUpdateDTO saveData1(LegalUpdateDTO obj)
        {
            return COMMM.POSTIssueManager(obj, "LegalUpdateFacade/saveData1/");
        }
        public LegalUpdateDTO validation(LegalUpdateDTO obj)
        {
            return COMMM.POSTIssueManager(obj, "LegalUpdateFacade/validation/");
        }
    }
}
