using PreadmissionDTOs.com.vaps.Legal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface LegalUpdateInterface
    {
        LegalUpdateDTO loaddata(LegalUpdateDTO dto);

        LegalUpdateDTO saveData1(LegalUpdateDTO data);
        Task<LegalUpdateDTO> validation(LegalUpdateDTO data);
    }
}
