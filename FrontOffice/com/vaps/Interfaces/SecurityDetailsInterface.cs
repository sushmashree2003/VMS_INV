using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Interfaces
{
     public interface SecurityDetailsInterface
    {
        SecurityDetailsDTO getdata(SecurityDetailsDTO data);
        SecurityDetailsDTO getalldetails(SecurityDetailsDTO data);
        SecurityDetailsDTO edittab1(SecurityDetailsDTO data);
        SecurityDetailsDTO deactive(SecurityDetailsDTO data);
        SecurityDetailsDTO Get_StateData(SecurityDetailsDTO data);
    }
}
