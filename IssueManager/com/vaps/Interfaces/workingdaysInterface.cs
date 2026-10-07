using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface workingdaysInterface
    {
        workingdays_DTO getdetails(workingdays_DTO dTO);
        workingdays_DTO saverecord(workingdays_DTO data);
        workingdays_DTO deactiveY(workingdays_DTO data);
        workingdays_DTO editrecord(workingdays_DTO data);

    }
}
