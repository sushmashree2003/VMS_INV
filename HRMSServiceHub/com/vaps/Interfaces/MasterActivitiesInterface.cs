using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServiceHub.com.vaps.Interfaces
{
    public interface MasterActivitiesInterface
    {

        MasterActivitiesDTO onloaddata(MasterActivitiesDTO data);
        MasterActivitiesDTO saverecord(MasterActivitiesDTO data);
        MasterActivitiesDTO deactiveY(MasterActivitiesDTO data);
    }
}
