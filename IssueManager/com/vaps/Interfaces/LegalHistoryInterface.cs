using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface LegalHistoryInterface
    {
        LegalHistoryDTO saveDataHistory(LegalHistoryDTO data);
        Task<LegalHistoryDTO> checkvalidation1(LegalHistoryDTO data);        
    }
}
