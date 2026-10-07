using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface MastersProjectInterface
    {
        MastersProject_DTO getdetails(MastersProject_DTO dTO);
        MastersProject_DTO OnChangeInstitution(MastersProject_DTO data);
        MastersProject_DTO saverecord(MastersProject_DTO data);
        MastersProject_DTO deactiveY(MastersProject_DTO data);

    }
}
