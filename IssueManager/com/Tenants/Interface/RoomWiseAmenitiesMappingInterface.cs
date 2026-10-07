using PreadmissionDTOs.com.vaps.IssueManager.Tenants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface RoomWiseAmenitiesMappingInterface
    {

        RoomWiseAmenitiesMappingDTO getloaddata(RoomWiseAmenitiesMappingDTO data);
        RoomWiseAmenitiesMappingDTO savedetails(RoomWiseAmenitiesMappingDTO data);
        RoomWiseAmenitiesMappingDTO deactive(RoomWiseAmenitiesMappingDTO data);
        RoomWiseAmenitiesMappingDTO Edit(RoomWiseAmenitiesMappingDTO data);

    }
}
