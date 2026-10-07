using CommonLibrary;
using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Legal
{
    public class LegalHistoryDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LegalHistoryDTO, LegalHistoryDTO> COMMM = new CommonDelegate<LegalHistoryDTO, LegalHistoryDTO>();
        
        public LegalHistoryDTO saveDataHistory(LegalHistoryDTO obj)
        {
            return COMMM.POSTIssueManager(obj, "LegalHistoryFacade/saveDataHistory/");
        }
        public LegalHistoryDTO checkvalidation1(LegalHistoryDTO obj)
        {
            return COMMM.POSTIssueManager(obj, "LegalHistoryFacade/checkvalidation1/");
        }
      



        }
    }

