using CommonLibrary;
using IVRMUX.Controllers;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Controllers.com.vapstech.FrontOffice
{
    public class SecurityRoasterDelegate
    {
        CommonDelegate<SecurityRoasterDTO, SecurityRoasterDTO> comm = new CommonDelegate<SecurityRoasterDTO, SecurityRoasterDTO>();
        public SecurityRoasterDTO getdata(SecurityRoasterDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityRoasterFacade/getdata");
        }
        public SecurityRoasterDTO getalldetails(SecurityRoasterDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityRoasterFacade/getalldetails/");
        }
        public SecurityRoasterDTO edittab1(SecurityRoasterDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityRoasterFacade/edittab1");
        }
        public SecurityRoasterDTO deactive(SecurityRoasterDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityRoasterFacade/deactive");
        }
        public SecurityRoasterDTO report(SecurityRoasterDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityRoasterFacade/report");
        }
        public SecurityRoasterDTO Binddata(SecurityRoasterDTO data)
        {
            return comm.POSTDataHolidayReport(data, "SecurityRoasterFacade/Binddata");
        }
    }
    
}
