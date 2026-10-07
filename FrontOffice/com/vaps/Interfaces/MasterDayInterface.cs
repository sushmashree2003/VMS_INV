using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Interfaces
{
    public interface MasterDayInterface
    {
        MasterDayDTO getalldetails(MasterDayDTO data);
        MasterDayDTO savedetails(MasterDayDTO data);
        MasterDayDTO deactive(MasterDayDTO data);
    }
}
