using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.FrontOffice
{
    public class MasterHolidayWorkingTypeDelegate
    {
        CommonDelegate<MasterHolidayWorkingTypeDTO, MasterHolidayWorkingTypeDTO> COMFRNT = new CommonDelegate<MasterHolidayWorkingTypeDTO, MasterHolidayWorkingTypeDTO>();

        public MasterHolidayWorkingTypeDTO getdata(MasterHolidayWorkingTypeDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "MasterHolidayWorkingTypeFacade/getalldetails/");
        }

        public MasterHolidayWorkingTypeDTO savedetails(MasterHolidayWorkingTypeDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "MasterHolidayWorkingTypeFacade/savedetails/");
        }


    }


}
