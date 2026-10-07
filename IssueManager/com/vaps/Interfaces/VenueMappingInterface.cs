using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
   public interface VenueMappingInterface
    {
        VenueMapping_DTO getdetails(VenueMapping_DTO dTO);
        VenueMapping_DTO saverecord(VenueMapping_DTO data);
        VenueMapping_DTO deactiveY(VenueMapping_DTO data);

    }
}
