using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class FoMasterDayDelegate
    {
        CommonDelegate<MasterDayDTO, MasterDayDTO> COMFRNT = new CommonDelegate<MasterDayDTO, MasterDayDTO>();

        public MasterDayDTO getalldetails(MasterDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "MasterDayFacade/getalldetails/");
        }

        public MasterDayDTO savedetails(MasterDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "MasterDayFacade/savedetails/");
        }

        public MasterDayDTO deactive(MasterDayDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "MasterDayFacade/deactive/");
       }

    }
}
