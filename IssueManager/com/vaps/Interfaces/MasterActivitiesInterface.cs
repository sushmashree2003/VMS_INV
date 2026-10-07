using PreadmissionDTOs.com.vaps.IssueManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueManager.com.vaps.Interfaces
{
    public interface MasterActivitiesInterface
    {
        MasterActivities_DTO getdetails(MasterActivities_DTO dTO);
        MasterActivities_DTO saverecord(MasterActivities_DTO data);
        MasterActivities_DTO deactiveY(MasterActivities_DTO data);

    }
}
