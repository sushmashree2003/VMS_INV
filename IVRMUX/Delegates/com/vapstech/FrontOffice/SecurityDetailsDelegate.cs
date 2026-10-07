using CommonLibrary;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    public class SecurityDetailsDelegate
    {
        CommonDelegate<SecurityDetailsDTO, SecurityDetailsDTO> comm = new CommonDelegate<SecurityDetailsDTO, SecurityDetailsDTO>();
        public SecurityDetailsDTO getdata(SecurityDetailsDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityDetailsFacade/getdata");
        }
        public SecurityDetailsDTO getalldetails(SecurityDetailsDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityDetailsFacade/getalldetails/");
        }
        public SecurityDetailsDTO edittab1(SecurityDetailsDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityDetailsFacade/edittab1");
        }
        public SecurityDetailsDTO deactive(SecurityDetailsDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityDetailsFacade/deactive");
        }
        public SecurityDetailsDTO Get_StateData(SecurityDetailsDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityDetailsFacade/Get_StateData");
        }
    }
}
