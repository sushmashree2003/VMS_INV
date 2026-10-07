using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface MasterHolidayWorkingTypeInterface
    {
        MasterHolidayWorkingTypeDTO getdata(MasterHolidayWorkingTypeDTO data);

        MasterHolidayWorkingTypeDTO savedetails(MasterHolidayWorkingTypeDTO data);
    }
}
