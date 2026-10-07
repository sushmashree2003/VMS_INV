using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface MasterVenuesInterface
    {
        MasterVenues_DTO getdetails(MasterVenues_DTO dTO);
        MasterVenues_DTO saverecord(MasterVenues_DTO data);
        MasterVenues_DTO deactiveY(MasterVenues_DTO data);

    }
}
