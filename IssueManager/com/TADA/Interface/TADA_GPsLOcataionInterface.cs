using PreadmissionDTOs.com.vaps.VMS.TADA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.TADA.Interface
{
   public interface TADA_GPsLOcataionInterface
    {
        TADA_GPsLOcataionDTO saveLocation(TADA_GPsLOcataionDTO data);
        TADA_GPsLOcataionDTO GetLocation(TADA_GPsLOcataionDTO data);
        
    }
}
