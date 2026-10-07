using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface MastersPrioritiesInterface
    {
        MastersPriorities_DTO getdetails(MastersPriorities_DTO dTO);
        MastersPriorities_DTO saverecord(MastersPriorities_DTO data);
        MastersPriorities_DTO deactiveY(MastersPriorities_DTO data);

    }
}
