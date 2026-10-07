using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
    public interface SecurityRoasterInterface
    {
        SecurityRoasterDTO getdata(SecurityRoasterDTO data);
        SecurityRoasterDTO getalldetails(SecurityRoasterDTO data);
        SecurityRoasterDTO edittab1(SecurityRoasterDTO data);
        SecurityRoasterDTO deactive(SecurityRoasterDTO data);
        SecurityRoasterDTO report(SecurityRoasterDTO data);
        SecurityRoasterDTO Binddata(SecurityRoasterDTO data);
    }
}
